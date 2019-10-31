using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Gameframe.GUI.Tween
{
    public static class TweenExtensions
    {
        private static CancellationTokenSource _cancellationTokenSource;
        private static Dictionary<int, TweenData> _tweenDict;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            //Setup Tween System
            Application.quitting += ApplicationOnQuitting;
            _cancellationTokenSource = new CancellationTokenSource();
            _tweenDict = new Dictionary<int, TweenData>();
        }

        private static void ApplicationOnQuitting()
        {
            //Tear down Tween system
            Application.quitting -= ApplicationOnQuitting;

            //Cancel all tweens
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            _tweenDict = null;
        }

        public static void CancelAllTweens()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            _tweenDict.Clear();
        }

        public static void DoKillTweens(this GameObject obj)
        {
            CancelTweensForId(obj.GetInstanceID());
        }

        public static void DoKillTweens(this Component obj)
        {
            CancelTweensForId(obj.GetInstanceID());
        }

        public static async Task DoColorAsync(this TextMeshProUGUI text, Color toColor, float duration)
        {
            var startColor = text.color;
            await DoTweenAsync(text.gameObject.GetInstanceID(), duration,
                (t) => { text.color = Color.Lerp(startColor, toColor, t); });
        }

        public static async void DoColor(this TextMeshProUGUI text, Color toColor, float duration)
        {
            await text.DoColorAsync(toColor, duration);
        }

        public static async Task DoAnchorPosYAsync(this RectTransform rectTransform, float position, float duration)
        {
            var startPosY = rectTransform.anchoredPosition.y;
            await DoTweenAsync(rectTransform.gameObject.GetInstanceID(), duration, (t) =>
            {
                var pt = rectTransform.anchoredPosition;
                pt.y = Mathf.Lerp(startPosY, position, t);
                rectTransform.anchoredPosition = pt;
            });
        }

        public static async void DoAnchorPosY(this RectTransform rectTransform, float position, float duration)
        {
            await rectTransform.DoAnchorPosYAsync(position, duration);
        }

        private static async Task DoTweenAsync(int id, float duration, Action<float> action)
        {
            await DoTweenAsync(id, duration, _cancellationTokenSource.Token, action);
        }

        private static async Task DoTweenAsync(int id, float duration, CancellationToken cancellationToken,
            Action<float> action)
        {
            var instanceCancellationToken = StartTween(id);

            float t = 0;
            action?.Invoke(0);

            while (t < duration && Application.isPlaying)
            {
                await Task.Yield();

                if (instanceCancellationToken.IsCancellationRequested || cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                t += Time.deltaTime;
                action?.Invoke(Mathf.InverseLerp(0, duration, t));
            }

            action?.Invoke(1);

            CompleteTween(id);
        }

        private static CancellationToken StartTween(int id)
        {
            var tweenData = GetTweenData(id);
            tweenData.count++;
            return tweenData.tokenSource.Token;
        }

        private static void CompleteTween(int id)
        {
            if (!_tweenDict.TryGetValue(id, out var tweenData))
            {
                return;
            }

            tweenData.count--;

            if (tweenData.count != 0)
            {
                return;
            }

            tweenData.tokenSource.Dispose();
            tweenData.tokenSource = null;
            _tweenDict.Remove(id);
        }

        private static void CancelTweensForId(int id)
        {
            if (!_tweenDict.TryGetValue(id, out var tweenData))
            {
                return;
            }

            tweenData.tokenSource.Cancel();
            tweenData.tokenSource.Dispose();
            tweenData.tokenSource = null;
            tweenData.count = -1;
            _tweenDict.Remove(id);
        }

        private static TweenData GetTweenData(int id)
        {
            if (_tweenDict.TryGetValue(id, out var tweenData))
            {
                return tweenData;
            }

            tweenData = new TweenData();
            tweenData.tokenSource = new CancellationTokenSource();
            tweenData.count = 0;
            _tweenDict.Add(id, tweenData);
            return tweenData;
        }

        private class TweenData
        {
            public CancellationTokenSource tokenSource;
            public int count;
        }
    }
}