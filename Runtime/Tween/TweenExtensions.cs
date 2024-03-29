﻿using System;
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

        private static CancellationToken CancellationToken => (_cancellationTokenSource != null) ? _cancellationTokenSource.Token : CancellationToken.None;

        private static bool CanTween => CanTweenPredicate.Invoke() && _tweenDict != null;

        private static Func<bool> _canTweenPredicate = DefaultCanTweenPredicate;
        public static Func<bool> CanTweenPredicate
        {
            get => _canTweenPredicate;
            set => _canTweenPredicate = value ?? DefaultCanTweenPredicate;
        }

        private static bool DefaultCanTweenPredicate()
        {
            return Application.isPlaying && !CancellationToken.IsCancellationRequested;
        }

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
            _tweenDict?.Clear();
        }

        public static void DoKillTweens(this GameObject obj)
        {
            CancelTweensForId(obj.GetInstanceID());
        }

        public static void DoKillTweens(this Component obj)
        {
            CancelTweensForId(obj.gameObject.GetInstanceID());
        }

        public static void DoKillTweens(this UnityEngine.Object obj)
        {
            CancelTweensForId(obj.GetInstanceID());
        }

        public static async Task DoTweenAsync(int id, float duration, Action<float> action, Easing easeType = Easing.Linear, AnimationCurve customCurve = null)
        {
            await DoTweenAsync(id, duration, CancellationToken, action, easeType, customCurve);
        }

        public static async Task DoPunchTweenAsync(int id, float duration, Action<float> action, Easing easeType = Easing.Linear, AnimationCurve customCurve = null)
        {
            await DoPunchTweenAsync(id, duration, CancellationToken, action, easeType, customCurve);
        }

        public static Task DoTweenAsync(int id, float duration, CancellationToken cancellationToken, Action<float> action, Easing easeType = Easing.Linear, AnimationCurve customCurve = null)
        {
            return DoTweenAsyncWithLerp(InverseLerpFloat, id, duration, cancellationToken, action, easeType, customCurve);
        }

        public static Task DoPunchTweenAsync(int id, float duration, CancellationToken cancellationToken, Action<float> action, Easing easeType = Easing.Linear, AnimationCurve customCurve = null)
        {
            return DoTweenAsyncWithLerp(PunchInverseLerpFloat, id, duration, cancellationToken, action, easeType, customCurve);
        }

        public static async Task DoTweenAsyncWithLerp(Func<float,float,float,float> lerpMethod, int id, float duration, CancellationToken cancellationToken, Action<float> action, Easing easeType = Easing.Linear, AnimationCurve customCurve = null)
        {
            if (!CanTween)
            {
                return;
            }

            var instanceCancellationToken = StartTween(id);

            float t = 0;
            var ease = easeType != Easing.CustomCurve ? EaseFunctions.Get(easeType) : customCurve.Evaluate;

            action?.Invoke(ease.Invoke(0));

            while (t < duration && CanTween)
            {
                await Task.Yield();

                if (instanceCancellationToken.IsCancellationRequested || cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                t += Time.deltaTime;
                action?.Invoke(ease.Invoke(lerpMethod(0, duration, t)));
            }

            //Just exit immediately if we've stopped
            if (!CanTween)
            {
                return;
            }

            action?.Invoke(ease.Invoke(lerpMethod(0, duration, duration)));

            CompleteTween(id);
        }

        private static float InverseLerpFloat(float a, float b, float t)
        {
            return Mathf.InverseLerp(a, b, t);
        }

        private static float PunchInverseLerpFloat(float a, float b, float t)
        {
            var r =  Mathf.InverseLerp(a, b, t);
            r *= 2;
            if (r > 1)
            {
                r = 2 - r;
            }
            return r;
        }

        private static CancellationToken StartTween(int id)
        {
            var tweenData = GetTweenData(id);
            tweenData.count++;
            return tweenData.tokenSource.Token;
        }

        private static void CompleteTween(int id)
        {
            if (_tweenDict == null)
            {
                return;
            }

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

        public static void CancelTweensForId(int id)
        {
            if (_tweenDict == null || !_tweenDict.TryGetValue(id, out var tweenData))
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
