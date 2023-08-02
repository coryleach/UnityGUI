using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.Tween
{
    public static class TransformTweenExtensions
    {
        public static async void DoPosX(this Transform transform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await transform.DoPosXAsync(position, duration, easing, customCurve);
        }

        public static async void DoPosY(this Transform transform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await transform.DoPosYAsync(position, duration, easing, customCurve);
        }

        public static async void DoPosZ(this Transform transform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await transform.DoPosYAsync(position, duration, easing, customCurve);
        }

        public static async Task DoPosXAsync(this Transform transform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var start = transform.position.x;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = transform.position;
                pt.x = Mathf.Lerp(start, position, t);
                transform.position = pt;
            }, easing, customCurve);
        }

        public static async Task DoPosYAsync(this Transform transform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var start = transform.position.y;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = transform.position;
                pt.y = Mathf.Lerp(start, position, t);
                transform.position = pt;
            }, easing, customCurve);
        }

        public static async Task DoPosZAsync(this Transform transform, float position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var start = transform.position.z;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = transform.position;
                pt.z = Mathf.Lerp(start, position, t);
                transform.position = pt;
            }, easing, customCurve);
        }

        public static async Task DoMoveAsync(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var startPos = transform.position;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = Vector3.Lerp(startPos, position, t);
                transform.position = pt;
            }, easing, customCurve);
        }

        public static async void DoMove(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await transform.DoMoveAsync(position, duration, easing, customCurve);
        }

        public static async Task DoLocalMoveAsync(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var startPos = transform.localPosition;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = Vector3.Lerp(startPos, position, t);
                transform.localPosition = pt;
            }, easing, customCurve);
        }

        public static async void DoLocalMove(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await transform.DoLocalMoveAsync(position, duration, easing, customCurve);
        }

        public static async Task DoPunchPositionAsync(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            var startPos = transform.position;
            await TweenExtensions.DoPunchTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = Vector3.Lerp(startPos, position, t);
                transform.position = pt;
            }, easing, customCurve);
        }

        public static async void DoPunchPosition(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear, AnimationCurve customCurve = null)
        {
            await transform.DoPunchPositionAsync(position, duration, easing, customCurve);
        }

    }
}
