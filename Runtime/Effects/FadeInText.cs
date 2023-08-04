using TMPro;
using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class FadeInText : PlayableTextMeshEffect, ITextMeshColorEffect
    {
        [SerializeField]
        private bool smooth;

        protected override void AddToManager(TextMeshEffectTMPro effectManager)
        {
            effectManager.AddColorEffect(this);
        }

        protected override void RemoveFromManager(TextMeshEffectTMPro effectManager)
        {
            effectManager.RemoveColorEffect(this);
        }

        public void UpdateColorEffect(TMP_CharacterInfo charInfo, ref EffectData data)
        {
            var left = GetEasedTime(ref data);
            var leftAlpha = (byte) Mathf.Round(255 * left);
            data.SetAlpha(0,leftAlpha);
            data.SetAlpha(1,leftAlpha);

            if (smooth)
            {
                var right = GetEasedTime(ref data, -delayPerCharacter);
                var rightAlpha = (byte) Mathf.Round(255 * right);
                data.SetAlpha(2,rightAlpha);
                data.SetAlpha(3,rightAlpha);
            }
            else
            {
                data.SetAlpha(2,leftAlpha);
                data.SetAlpha(3,leftAlpha);
            }
        }
    }
}
