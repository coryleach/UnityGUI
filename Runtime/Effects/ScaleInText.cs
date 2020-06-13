using Gameframe.GUI.Tween;
using TMPro;
using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class ScaleInText : VertexTextMeshEffect
    {
        [SerializeField]
        private Vector3 startScale = Vector3.zero;
        
        [SerializeField]
        protected Vector3 endScale = Vector3.one;

        public override void UpdateVertexEffect(TMP_CharacterInfo charInfo, ref EffectData data)
        {
            var t = Mathf.Clamp01(progress - data.Index);
            t = EaseFunctions.Ease(easeType, t);
            var delta = endScale - startScale;
            data.LocalScale = startScale + delta * t;
        }
    }
}


