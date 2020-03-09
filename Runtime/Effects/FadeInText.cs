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
        private float charactersPerSecond = 15;
        public float CharacterPerSecond
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
            var left = Mathf.Clamp01(progress - charInfo.index);
            var right = Mathf.Clamp01(progress - (charInfo.index+1));

            data.color0.a = (byte) (255 * left);
            data.color1.a = (byte) (255 * left);
            data.color2.a = (byte) (255 * right);
            data.color3.a = (byte) (255 * right);
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


