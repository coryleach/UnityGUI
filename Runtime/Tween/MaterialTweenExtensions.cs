using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.Tween
{
    public static class MaterialTweenExtensions
    {
        public static async Task DoFloatAsync(this Material material, int propertyId, float startValue, float endValue, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await TweenExtensions.DoTweenAsync(material.GetInstanceID(), duration, (t) =>
            {
                material.SetFloat(propertyId, Mathf.Lerp(startValue, endValue, t));
            }, easing, customCurve);
        }

        public static async Task DoFloatAsync(this Material material, string propertyName, float startValue, float endValue, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var propertyId = Shader.PropertyToID(propertyName);
            await TweenExtensions.DoTweenAsync(material.GetInstanceID(), duration, (t) =>
            {
                material.SetFloat(propertyId, Mathf.Lerp(startValue, endValue, t));
            }, easing, customCurve);
        }
    }
}