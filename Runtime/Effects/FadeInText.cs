using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class FadeInText : MonoBehaviour, ITextMeshColorEffect
    {
        [SerializeField] 
        private TextMeshEffectTMPro effectManager;
        
        [SerializeField] 
        protected bool playOnEnable = false;

        [SerializeField] 
        private int charactersPerSecond = 15;
        public int CharacterPerSecond
        {
            get => charactersPerSecond;
            set => charactersPerSecond = value;
        }
        
        private string currentMessage = string.Empty;
        private Coroutine coroutine = null;
        private float progress = 0;

        public bool IsPlaying => coroutine != null;
        
        [SerializeField]
        protected UnityEvent onComplete = new UnityEvent();
        public UnityEvent OnComplete => onComplete;

        private void OnEnable()
        {
            if (playOnEnable)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            Finish();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            Play(effectManager.Text.text);
        }
        
        public void Play(string message)
        {
            currentMessage = message;
            if (coroutine != null)
            {
                Finish();
            }
            coroutine = StartCoroutine(RunAnimation());
        }

        public void Finish()
        {
            if (coroutine != null)
            {
                effectManager.RemoveColorEffect(this);
                StopCoroutine(coroutine);
                onComplete.Invoke();
            }
            coroutine = null;
        }
        
        private IEnumerator RunAnimation()
        {
            effectManager.AddColorEffect(this);
            
            progress = 0;
            while (progress < currentMessage.Length)
            {
                progress += Time.deltaTime * charactersPerSecond;
                yield return null;
            }
            
            Finish();
        }

        public void UpdateColorEffect(TMP_CharacterInfo charInfo, ref EffectData data)
        {
            data.alpha = Mathf.Clamp01(progress - charInfo.index);
        }

        private void OnValidate()
        {
            if (effectManager == null)
            {
                effectManager = GetComponent<TextMeshEffectTMPro>();
            }
        }
        
    }
}


