using Gameframe.GUI.Tween;
using TMPro;
using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class FadeInText : PlayableTextMeshEffect, ITextMeshColorEffect
    {
        [SerializeField] 
        private bool smooth = false;
        
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
            var left = EaseFunctions.Ease(easeType,1 - Mathf.Clamp01(charInfo.index - (progress - 0.5f)));
            data.color0.a = (byte) Mathf.Round(255 * left);
            data.color1.a = (byte) Mathf.Round(255 * left);
            
            if (smooth)
            {
                var right = EaseFunctions.Ease(easeType, 1 - Mathf.Clamp01(charInfo.index - (progress - 1f)));
                data.color2.a = (byte) Mathf.Round(255 * right);
                data.color3.a = (byte) Mathf.Round(255 * right);
            }
            else
            {
                data.color2.a = (byte) Mathf.Round(255 * left);
                data.color3.a = (byte) Mathf.Round(255 * left);
            }
        }
    }
}


