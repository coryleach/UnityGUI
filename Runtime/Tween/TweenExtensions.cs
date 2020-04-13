using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        
        public static async Task DoTweenAsync(int id, float duration, Action<float> action, Easing easeType = Easing.Linear)
        {
            await DoTweenAsync(id, duration, _cancellationTokenSource.Token, action, easeType);
        }

        public static async Task DoTweenAsync(int id, float duration, CancellationToken cancellationToken,
            Action<float> action, Easing easeType = Easing.Linear)
        {
            var instanceCancellationToken = StartTween(id);

            float t = 0;
            var ease = EaseFunctions.Get(easeType);
            action?.Invoke(ease.Invoke(0));

            while (t < duration && Application.isPlaying)
            {
                await Task.Yield();

                if (instanceCancellationToken.IsCancellationRequested || cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                t += Time.deltaTime;
                action?.Invoke(ease.Invoke(Mathf.InverseLerp(0, duration, t)));
            }

            //Just exit immediately if we've stopped playing in editor
            if (!Application.isPlaying)
            {
                return;
            }
            
            action?.Invoke(ease.Invoke(1));

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