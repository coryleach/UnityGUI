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
            var easeTime = GetEasedTime(ref data);
            var delta = endOffset - startOffset;
            data.LocalPosition = startOffset + delta * easeTime;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (characterAnimationDuration < 0.1f)
            {
                characterAnimationDuration = 0.1f;
            }
        }
    }
}
