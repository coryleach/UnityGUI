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
    public struct EffectData : IEquatable<EffectData>
    {
        [SerializeField] private int index;
        [SerializeField] private Vector3 localPosition;
        [SerializeField] private Quaternion localRotation;
        [SerializeField] private Vector3 localScale;
        [SerializeField] private Color32 color0;
        [SerializeField] private Color32 color1;
        [SerializeField] private Color32 color2;
        [SerializeField] private Color32 color3;
        
        public int Index
        {
            get => index;
            set => index = value;
        }
        public Vector3 LocalPosition
        {
            get => localPosition;
            set => localPosition = value;
        }
        public Quaternion LocalRotation
        {
            get => localRotation;
            set => localRotation = value;
        }
        public Vector3 LocalScale
        {
            get => localScale;
            set => localScale = value;
        }
        public Color32 Color0
        {
            get => color0;
            set => color0 = value;
        }
        public Color32 Color1
        {
            get => color1;
            set => color1 = value;
        }
        public Color32 Color2
        {
            get => color2;
            set => color2 = value;
        }
        public Color32 Color3
        {
            get => color3;
            set => color3 = value;
        }

        public void SetAlpha(int colorIndex, byte alpha)
        {
            switch (colorIndex)
            {
                case 0:
                    color0.a = alpha;
                    break;
                case 1:
                    color1.a = alpha;
                    break;
                case 2:
                    color2.a = alpha;
                    break;
                case 3:
                    color3.a = alpha;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Equals(EffectData other)
        {
            return index == other.index && localPosition.Equals(other.localPosition) && localRotation.Equals(other.localRotation) && localScale.Equals(other.localScale) && color0.Equals(other.color0) && color1.Equals(other.color1) && color2.Equals(other.color2) && color3.Equals(other.color3);
        }

        public override bool Equals(object obj)
        {
            return obj is EffectData other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                var hashCode = index;
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ localPosition.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ localRotation.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ localScale.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ color0.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ color1.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ color2.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ color3.GetHashCode();
                return hashCode;
            }
        }
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
                effectDatas[i].Index = i;
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

        private static void ResetEffectData(ref EffectData data)
        {
            data.LocalPosition = Vector3.zero;
            data.LocalRotation = Quaternion.identity;
            data.LocalScale = Vector3.one;
            data.Color0 = new Color32(1,1,1,1);
            data.Color1 = new Color32(1,1,1,1);
            data.Color2 = new Color32(1,1,1,1);
            data.Color3 = new Color32(1,1,1,1);
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
            var matrix = Matrix4x4.TRS(anim.LocalPosition, anim.LocalRotation, anim.LocalScale);
                    
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

            colorBottomLeft.a = effect.Color0.a;
            colorTopLeft.a = effect.Color1.a;
            colorTopRight.a = effect.Color2.a;
            colorBottomRight.a = effect.Color3.a;
            
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

