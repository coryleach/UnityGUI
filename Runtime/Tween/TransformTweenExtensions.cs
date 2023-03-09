using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.Tween
{
    public static class TransformTweenExtensions
    {
        public static async void DoPosX(this Transform transform, float position, float duration, Easing easing = Easing.Linear)
        {
            await transform.DoPosXAsync(position, duration, easing).ConfigureAwait(false);
        }

        public static async void DoPosY(this Transform transform, float position, float duration, Easing easing = Easing.Linear)
        {
            await transform.DoPosYAsync(position, duration, easing).ConfigureAwait(false);
        }

        public static async void DoPosZ(this Transform transform, float position, float duration, Easing easing = Easing.Linear)
        {
            await transform.DoPosYAsync(position, duration, easing).ConfigureAwait(false);
        }

        public static async Task DoPosXAsync(this Transform transform, float position, float duration, Easing easing = Easing.Linear)
        {
            var start = transform.position.x;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = transform.position;
                pt.x = Mathf.Lerp(start, position, t);
                transform.position = pt;
            }, easing);
        }

        public static async Task DoPosYAsync(this Transform transform, float position, float duration, Easing easing = Easing.Linear)
        {
            var start = transform.position.y;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = transform.position;
                pt.y = Mathf.Lerp(start, position, t);
                transform.position = pt;
            }, easing);
        }

        public static async Task DoPosZAsync(this Transform transform, float position, float duration, Easing easing = Easing.Linear)
        {
            var start = transform.position.z;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = transform.position;
                pt.z = Mathf.Lerp(start, position, t);
                transform.position = pt;
            }, easing);
        }

        public static async Task DoMoveAsync(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear)
        {
            var startPos = transform.position;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = Vector3.Lerp(startPos, position, t);
                transform.position = pt;
            }, easing);
        }

        public static async void DoMove(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear)
        {
            await transform.DoMoveAsync(position, duration, easing).ConfigureAwait(false);
        }

        public static async Task DoLocalMoveAsync(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear)
        {
            var startPos = transform.position;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = Vector3.Lerp(startPos, position, t);
                transform.localPosition = pt;
            }, easing);
        }

        public static async void DoLocalMove(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear)
        {
            await transform.DoLocalMoveAsync(position, duration, easing).ConfigureAwait(false);
        }

        public static async Task DoPunchPosition(this Transform transform, Vector3 position, float duration, Easing easing = Easing.Linear)
        {
            var startPos = transform.position;
            await TweenExtensions.DoTweenAsync(transform.gameObject.GetInstanceID(), duration, (t) =>
            {
                t *= 2;
                if (t > 1)
                {
                    t = 2 - t;
                }

                var pt = Vector3.Lerp(startPos, position, t);
                transform.localPosition = pt;
            }, easing);
        }

        
    }
}
