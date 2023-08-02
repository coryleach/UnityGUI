using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.Tween
{
    public static class RectTransformTweenExtensions
    {
        public static async Task DoAnchorPosYAsync(this RectTransform rectTransform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var startPosY = rectTransform.anchoredPosition.y;
            await TweenExtensions.DoTweenAsync(rectTransform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = rectTransform.anchoredPosition;
                pt.y = Mathf.Lerp(startPosY, position, t);
                rectTransform.anchoredPosition = pt;
            }, easing, customCurve);
        }

        public static async void DoAnchorPosY(this RectTransform rectTransform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await rectTransform.DoAnchorPosYAsync(position, duration, easing, customCurve);
        }
    }
}
