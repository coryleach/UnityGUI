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
    
    [Serializable]
    public struct EffectData
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public float alpha;
    }
    
    /// <summary>
    /// Manager for effects that modify the text mesh
    /// </summary>
    public class TextMeshEffectTMPro : UIBehaviour
    {
        [SerializeField] 
        protected TextMeshProUGUI text;

        public TextMeshProUGUI Text => text;
        
        public Vector2 pivot = new Vector2(0.5f,0.5f);

        private EffectData[] effectDatas = new EffectData[0];
        private TMP_MeshInfo[] meshCache = null;
        private Coroutine coroutine = null;
        private bool dirtyVerts = false;
        private bool dirtyColors = false;

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
            dirtyVerts = true;
        }

        public void AddColorEffect(ITextMeshColorEffect colorEffect)
        {
            colorEffects.Add(colorEffect);
            CheckCoroutine();
        }

        public void RemoveColorEffect(ITextMeshColorEffect colorEffect)
        {
            colorEffects.Remove(colorEffect);
            dirtyColors = true;
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
                ResetEffectData(ref effectDatas[i]);
            }
        }

        private void ValidateTransforms()
        {
            if (effectDatas != null && text.textInfo.characterCount <= effectDatas.Length)
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
            coroutine = StartCoroutine(Run());
        }
        
        private IEnumerator Run()
        {
            while (colorEffects.Count + vertEffects.Count > 0)
            {
                ApplyAnimations();
                yield return null;
            }
            ApplyAnimations();
            coroutine = null;
        }

        private void ApplyAnimations()
        {
            text.ForceMeshUpdate();
            
            ValidateTransforms();

            var flags = TMP_VertexDataUpdateFlags.None;
            
            if (vertEffects.Count > 0 || dirtyVerts)
            {
                flags |= TMP_VertexDataUpdateFlags.Vertices;
            }

            if (colorEffects.Count > 0 || dirtyColors)
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
                
                if (vertEffects.Count > 0 || dirtyVerts)
                {
                    ApplyTransforms(charInfo);
                    dirtyVerts = false;
                }

                if (colorEffects.Count > 0 || dirtyColors)
                {
                    ApplyColors(charInfo);
                    dirtyColors = false;
                }
            }
            
            text.UpdateVertexData(flags);
        }

        private void ResetEffectData(ref EffectData data)
        {
            data.localPosition = Vector3.zero;
            data.localRotation = Quaternion.identity;
            data.localScale = Vector3.one;
            data.alpha = 1;
        }
        
        private void ApplyTransforms(TMP_CharacterInfo charInfo)
        {
            var materialIndex = charInfo.materialReferenceIndex;
            var vertexIndex = charInfo.vertexIndex;
                    
            //Do Vertices
            var sourceVertices = meshCache[materialIndex].vertices;

            // Getting this from charInfo.vertex_TL, etc. yields the wrong values
            var sourceBottomLeft = sourceVertices[vertexIndex + 0];
            var sourceTopLeft = sourceVertices[vertexIndex + 1];
            var sourceTopRight = sourceVertices[vertexIndex + 2];
            var sourceBottomRight = sourceVertices[vertexIndex + 3];
                    
            var offset = sourceBottomLeft + (sourceBottomRight - sourceBottomLeft) * pivot.x + (sourceTopLeft - sourceBottomLeft) * pivot.y;

            var anim = effectDatas[charInfo.index];
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
        
        private void ApplyColors(TMP_CharacterInfo charInfo)
        {
            var materialIndex = charInfo.materialReferenceIndex;
            var vertexIndex = charInfo.vertexIndex;
            var characterIndex = charInfo.index;

            var animInfo = effectDatas[characterIndex];
            
            //Do Colors
            var sourceColors = meshCache[materialIndex].colors32;
            var colorBottomLeft = sourceColors[vertexIndex + 0];
            var colorTopLeft = sourceColors[vertexIndex + 1];
            var colorTopRight = sourceColors[vertexIndex + 2];
            var colorBottomRight = sourceColors[vertexIndex + 3];

            colorTopLeft.a = (byte)(animInfo.alpha * 255);
            colorTopRight.a = (byte)(animInfo.alpha * 255);
            colorBottomLeft.a = (byte)(animInfo.alpha * 255);
            colorBottomRight.a = (byte)(animInfo.alpha * 255);
            
            var destinationColors = text.textInfo.meshInfo[materialIndex].colors32;
            destinationColors[vertexIndex + 0] = colorBottomLeft;
            destinationColors[vertexIndex + 1] = colorTopLeft;
            destinationColors[vertexIndex + 2] = colorTopRight;
            destinationColors[vertexIndex + 3] = colorBottomRight;
        }

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
        
    }

}

