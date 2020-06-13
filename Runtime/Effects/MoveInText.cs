using Gameframe.GUI.Tween;
using TMPro;
using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class MoveInText : VertexTextMeshEffect
    {
        [SerializeField]
        private Vector3 startOffset = Vector3.zero;
        
        [SerializeField]
        protected Vector3 endOffset = Vector3.one;

        public override void UpdateVertexEffect(TMP_CharacterInfo charInfo, ref EffectData data)
        {
            var t = Mathf.Clamp01(progress - data.Index);
            t = EaseFunctions.Ease(easeType, t);
            var delta = endOffset - startOffset;
            data.LocalPosition = startOffset + delta * t;
        }
    }
}


