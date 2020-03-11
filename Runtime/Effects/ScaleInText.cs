using Gameframe.GUI.Tween;
using TMPro;
using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class ScaleInText : PlayableTextMeshEffect, ITextMeshVertexEffect
    {
        [SerializeField]
        private Vector3 startScale = Vector3.zero;
        
        [SerializeField]
        protected Vector3 endScale = Vector3.one;

        protected override void AddToManager(TextMeshEffectTMPro effectManager)
        {
            effectManager.AddVertexEffect(this);
        }

        protected override void RemoveFromManager(TextMeshEffectTMPro effectManager)
        {
            effectManager.RemoveVertexEffect(this);
        }
       
        public void UpdateVertexEffect(TMP_CharacterInfo charInfo, ref EffectData data)
        {
            var t = Mathf.Clamp01(progress - charInfo.index);
            t = EaseFunctions.Ease(easeType, t);
            var delta = endScale - startScale;
            data.localScale = startScale + delta * t; //Vector3.Lerp(startScale, endScale, t );
        }
    }
}


