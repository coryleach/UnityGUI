using System.Collections;
using Gameframe.GUI.Tween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Gameframe.GUI
{
    public abstract class PlayableTextMeshEffect : MonoBehaviour, IPlayableTextMeshEffect
    {
        [SerializeField]
        private TextMeshEffectTMPro effectManager;

        [SerializeField]
        protected bool playOnEnable;

        [SerializeField]
        protected Easing easeType = Easing.Linear;

        public Easing EaseType
        {
            get => easeType;
            set => easeType = value;
        }

        [SerializeField]
        protected AnimationCurve customCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public AnimationCurve Curve
        {
            get => customCurve;
            set => customCurve = value;
        }

        [SerializeField][FormerlySerializedAs("charactersPerSecond")]
        protected float speed = 15;
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        [SerializeField][FormerlySerializedAs("startDelayMultiplier")]
        protected float delayPerCharacter = 1f;

        public float DelayPerCharacter
        {
            get => delayPerCharacter;
            set => delayPerCharacter = value;
        }

        [SerializeField]
        protected float characterAnimationDuration = 1f;

        public float CharacterAnimationDuration
        {
            get => characterAnimationDuration;
            set => characterAnimationDuration = value;
        }

        [SerializeField]
        protected UnityEvent onComplete = new UnityEvent();
        public UnityEvent OnComplete => onComplete;

        private float _progress;

        private Coroutine _coroutine;

        public bool IsPlaying => _coroutine != null;

        protected float GetEasedTime(ref EffectData data, float offset = 0)
        {
            var characterStartTime = data.Index * delayPerCharacter;
            var rawTime = (_progress + offset - characterStartTime) / characterAnimationDuration;
            var easeTime = Ease(Mathf.Clamp01(rawTime));
            return easeTime;
        }

        /// <summary>
        /// Value goes from 1 to 0 instead of 0 to 1
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected float GetInverseEaseTime(ref EffectData data, float offset = 0)
        {
            var characterStartTime = data.Index * delayPerCharacter;
            var rawTime = (_progress + offset - characterStartTime) / characterAnimationDuration;
            var easeTime = Ease(1 - Mathf.Clamp01(rawTime));
            return easeTime;
        }

        protected void OnEnable()
        {
            if (playOnEnable)
            {
                Play();
            }
        }

        protected void OnDisable()
        {
            Finish();
        }

        public void Play()
        {
            if (_coroutine != null)
            {
                Finish();
            }
            _coroutine = StartCoroutine(Run());
        }

        public void Finish()
        {
            if (_coroutine != null)
            {
                RemoveFromManager(effectManager);
                StopCoroutine(_coroutine);
                onComplete.Invoke();
            }
            _coroutine = null;
        }

        protected abstract void AddToManager(TextMeshEffectTMPro effectManager);
        protected abstract void RemoveFromManager(TextMeshEffectTMPro effectManager);

        protected float Ease(float t)
        {
            return easeType == Easing.CustomCurve ? customCurve.Evaluate(t) : EaseFunctions.Ease(easeType, t);
        }

        private IEnumerator Run()
        {
            AddToManager(effectManager);
            yield return RunAnimation();
            Finish();
        }

        private IEnumerator RunAnimation()
        {
            var characterCount = effectManager.Text.textInfo.characterCount;
            _progress = 0;
            var duration = (delayPerCharacter * characterCount) + characterAnimationDuration;
            while (_progress < duration)
            {
                _progress += Time.deltaTime * speed;
                yield return null;
            }
        }

        protected virtual void OnValidate()
        {
            if (effectManager == null)
            {
                effectManager = GetComponent<TextMeshEffectTMPro>();
            }

            if (speed < 0)
            {
                speed = 1;
            }

            if (characterAnimationDuration < 0.01f)
            {
                characterAnimationDuration = 0.01f;
            }
        }
    }
}
