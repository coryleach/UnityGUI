using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameframe.GUI
{
    public interface ITextMeshVertexEffect
    {
        void UpdateVertexEffect(TMP_CharacterInfo charInfo, ref EffectData data);
    }

    public interface ITextMeshColorEffect
    {
        void UpdateColorEffect(TMP_CharacterInfo charInfo, ref EffectData data);
    }
    
    public interface IPlayableTextMeshEffect
    {
        bool IsPlaying { get; }
        void Play();
        void Finish();
    }
    
    [Serializable]
    public struct EffectData
    {
        public int index;
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public Color32 color0;
        public Color32 color1;
        public Color32 color2;
        public Color32 color3;
    }
    
    /// <summary>
    /// Manager for effects that modify the text mesh
    /// </summary>
    public class TextMeshEffectTMPro : UIBehaviour
    {
        [SerializeField] 
        protected TextMeshProUGUI text;

        public TextMeshProUGUI Text => text;
        
        [SerializeField] private Vector2 pivot = new Vector2(0.5f,0.5f);
        public Vector2 Pivot
        {
            get => pivot;
            set => pivot = value;
        }

        private EffectData[] effectDatas = new EffectData[0];
        private TMP_MeshInfo[] meshCache;
        private Coroutine coroutine;

        private readonly List<ITextMeshVertexEffect> vertEffects = new List<ITextMeshVertexEffect>();
        private readonly List<ITextMeshColorEffect> colorEffects = new List<ITextMeshColorEffect>();

        protected override void OnEnable()
        {
            base.OnEnable();
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
            text.ForceMeshUpdate();
            InitializeCharacterTransforms();
            Refresh();
            CheckCoroutine();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        
        public void AddVertexEffect(ITextMeshVertexEffect vertexEffect)
        {
            vertEffects.Add(vertexEffect);
            CheckCoroutine();
        }

        public void RemoveVertexEffect(ITextMeshVertexEffect vertexEffect)
        {
            vertEffects.Remove(vertexEffect);
        }

        public void AddColorEffect(ITextMeshColorEffect colorEffect)
        {
            colorEffects.Add(colorEffect);
            CheckCoroutine();
        }

        public void RemoveColorEffect(ITextMeshColorEffect colorEffect)
        {
            colorEffects.Remove(colorEffect);
        }

        private void OnTextChanged(object eventText)
        {
            if (text == null)
            {
                TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
                return;
            }
            
            if (!ReferenceEquals(eventText, text))
            {
                return;
            }

            Refresh();
        }

        private void Refresh()
        {
            meshCache = text.textInfo.CopyMeshInfoVertexData();
        }

        private void InitializeCharacterTransforms(int startIndex = 0)
        {
            for (var i = startIndex; i < effectDatas.Length; i++)
            {
                effectDatas[i].index = i;
                ResetEffectData(ref effectDatas[i]);
            }
        }

        private void ValidateTransforms()
        {
            if (effectDatas != null && text.textInfo.characterCount < effectDatas.Length)
            {
                return;
            }
            
            var old = effectDatas;
            var startIndex = 0 + (effectDatas?.Length ?? 0);
            effectDatas = new EffectData[text.textInfo.characterCount];
            old?.CopyTo(effectDatas,0);
            InitializeCharacterTransforms(startIndex);
        }
        
        private void CheckCoroutine()
        {
            if (coroutine != null)
            {
                return;
            }

            if (colorEffects.Count + vertEffects.Count > 0)
            {
                coroutine = StartCoroutine(Run());
            }
        }
        
        private IEnumerator Run()
        {
            //Early out if we have no work to do so we don't try to Apply animations when we shouldn't
            if (colorEffects.Count + vertEffects.Count == 0)
            {
                coroutine = null;
                yield break;
            }

            do
            {
                ApplyAnimations();
                yield return null;
            } while (colorEffects.Count + vertEffects.Count > 0);

            coroutine = null;
            
            //One last update when we have zero effects to reset the text mesh
            ApplyAnimations();
        }

        public void ApplyAnimations()
        {
            text.ForceMeshUpdate();

            if (meshCache == null)
            {
                meshCache = text.textInfo.CopyMeshInfoVertexData();
            }
            
            ValidateTransforms();

            var flags = TMP_VertexDataUpdateFlags.None;
            
            if (vertEffects.Count > 0)
            {
                flags |= TMP_VertexDataUpdateFlags.Vertices;
            }

            if (colorEffects.Count > 0)
            {
                flags |= TMP_VertexDataUpdateFlags.Colors32;
            }

            var characterCount = text.textInfo.characterCount;
            
            for (var i = 0; i < characterCount; i++)
            {
                var charInfo = text.textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                {
                    continue;
                }

                ResetEffectData(ref effectDatas[i]);
                
                foreach (var vertEffect in vertEffects)
                {
                    vertEffect.UpdateVertexEffect(charInfo,ref effectDatas[i]);
                }
                
                foreach (var colorEffect in colorEffects)
                {
                    colorEffect.UpdateColorEffect(charInfo,ref effectDatas[i]);
                }
                
                if (vertEffects.Count > 0)
                {
                    ApplyTransforms(charInfo, i);
                }

                if (colorEffects.Count > 0)
                {
                    ApplyColors(charInfo, i);
                }
            }
            
            text.UpdateVertexData(flags);
        }

        private void ResetEffectData(ref EffectData data)
        {
            data.localPosition = Vector3.zero;
            data.localRotation = Quaternion.identity;
            data.localScale = Vector3.one;
            data.color0 = new Color32(1,1,1,1);
            data.color1 = new Color32(1,1,1,1);
            data.color2 = new Color32(1,1,1,1);
            data.color3 = new Color32(1,1,1,1);
        }
        
        /// <summary>
        /// It turns out charInfo.index is the actual character position in the string
        /// If you have tags like <color> etc. it will be included in the index.
        /// Therefore we need to pass in the actual index from our loop here.
        /// </summary>
        /// <param name="charInfo">charInfo struct for character being modified</param>
        /// <param name="index">index of the character. This is excluding characters in tags.</param>
        private void ApplyTransforms(TMP_CharacterInfo charInfo, int index)
        {
            var materialIndex = charInfo.materialReferenceIndex;
            var vertexIndex = charInfo.vertexIndex;
                    
            //Do Vertices
            var sourceVertices = meshCache[materialIndex].vertices;

            var sourceBottomLeft = sourceVertices[vertexIndex + 0];
            var sourceTopLeft = sourceVertices[vertexIndex + 1];
            var sourceTopRight = sourceVertices[vertexIndex + 2];
            var sourceBottomRight = sourceVertices[vertexIndex + 3];
                    
            var offset = sourceBottomLeft + (sourceBottomRight - sourceBottomLeft) * pivot.x + (sourceTopLeft - sourceBottomLeft) * pivot.y;

            var anim = effectDatas[index];
            var matrix = Matrix4x4.TRS(anim.localPosition, anim.localRotation, anim.localScale);
                    
            var destinationTopLeft = matrix.MultiplyPoint3x4(sourceTopLeft - offset) + offset;
            var destinationTopRight = matrix.MultiplyPoint3x4(sourceTopRight - offset) + offset;
            var destinationBottomLeft = matrix.MultiplyPoint3x4(sourceBottomLeft - offset) + offset;
            var destinationBottomRight = matrix.MultiplyPoint3x4(sourceBottomRight - offset) + offset;
                    
            var destinationVertices = text.textInfo.meshInfo[materialIndex].vertices;
            destinationVertices[vertexIndex + 0] = destinationBottomLeft;
            destinationVertices[vertexIndex + 1] = destinationTopLeft;
            destinationVertices[vertexIndex + 2] = destinationTopRight;
            destinationVertices[vertexIndex + 3] = destinationBottomRight;
        }
        
        /// <summary>
        /// It turns out charInfo.index is the actual character position in the string
        /// If you have tags like <color> etc. it will be included in the index.
        /// Therefore we need to pass in the actual index from our loop here.
        /// </summary>
        /// <param name="charInfo">charInfo struct for character being modified</param>
        /// <param name="index">index of the character. This is excluding characters in tags.</param>
        private void ApplyColors(TMP_CharacterInfo charInfo, int index)
        {
            var materialIndex = charInfo.materialReferenceIndex;
            var vertexIndex = charInfo.vertexIndex;

            var effect = effectDatas[index];
            
            var sourceColors = meshCache[materialIndex].colors32;
            
            var colorBottomLeft = sourceColors[vertexIndex + 0];
            var colorTopLeft = sourceColors[vertexIndex + 1];
            var colorTopRight = sourceColors[vertexIndex + 2];
            var colorBottomRight = sourceColors[vertexIndex + 3];

            colorBottomLeft.a = effect.color0.a;
            colorTopLeft.a = effect.color1.a;
            colorTopRight.a = effect.color2.a;
            colorBottomRight.a = effect.color3.a;
            
            var destinationColors = text.textInfo.meshInfo[materialIndex].colors32;
            destinationColors[vertexIndex + 0] = colorBottomLeft;
            destinationColors[vertexIndex + 1] = colorTopLeft;
            destinationColors[vertexIndex + 2] = colorTopRight;
            destinationColors[vertexIndex + 3] = colorBottomRight;
        }

        
        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
            }
            pivot.x = Mathf.Clamp01(pivot.x);
            pivot.y = Mathf.Clamp01(pivot.y);
        }
        #endif
        
    }

}

