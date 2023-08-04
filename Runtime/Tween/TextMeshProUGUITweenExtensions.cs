using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Gameframe.GUI.Tween
{
    public static class TextMeshProUGUITweenExtensions
    {
        public static async Task DoColorAsync(this TextMeshProUGUI text, Color toColor, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var startColor = text.color;
            await TweenExtensions.DoTweenAsync(text.gameObject.GetInstanceID(), duration,
                (t) => { text.color = Color.Lerp(startColor, toColor, t); }, easing, customCurve);
        }

        public static async void DoColor(this TextMeshProUGUI text, Color toColor, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await text.DoColorAsync(toColor, duration, easing, customCurve);
        }
    }
}
