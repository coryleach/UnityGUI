using System.Collections;
using Gameframe.GUI.Tween;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI
{
    public abstract class PlayableTextMeshEffect : MonoBehaviour, IPlayableTextMeshEffect
    {
        [SerializeField]
        private TextMeshEffectTMPro effectManager;
        
        [SerializeField]
        protected bool playOnEnable = false;

        [SerializeField] 
        protected Easing easeType = Easing.Linear;

        [SerializeField] 
        protected float charactersPerSecond = 15;
        public float CharacterPerSecond
        {
            get => charactersPerSecond;
            set => charactersPerSecond = value;
        }
        
        [SerializeField]
        protected UnityEvent onComplete = new UnityEvent();
        public UnityEvent OnComplete => onComplete;
        
        protected float progress = 0;
        
        private Coroutine coroutine = null;

        public bool IsPlaying => coroutine != null;
        
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
            if (coroutine != null)
            {
                Finish();
            }
            coroutine = StartCoroutine(Run());
        }

        public void Finish()
        {
            if (coroutine != null)
            {
                RemoveFromManager(effectManager);
                StopCoroutine(coroutine);
                onComplete.Invoke();
            }
            coroutine = null;
        }

        protected abstract void AddToManager(TextMeshEffectTMPro effectManager);
        protected abstract void RemoveFromManager(TextMeshEffectTMPro effectManager);

        private IEnumerator Run()
        {
            AddToManager(effectManager);
            yield return RunAnimation();
            Finish();
        }
        
        private IEnumerator RunAnimation()
        {
            int characterCount = effectManager.Text.textInfo.characterCount;
            progress = 0;
            while (progress < characterCount)
            {
                progress += Time.smoothDeltaTime * charactersPerSecond;
                yield return null;
            }
        }
        
        private void OnValidate()
        {
            if (effectManager == null)
            {
                effectManager = GetComponent<TextMeshEffectTMPro>();
            }

            if (charactersPerSecond < 0)
            {
                charactersPerSecond = 1;
            }
        }
    }
}

