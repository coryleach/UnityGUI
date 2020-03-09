using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class ScaleInText : MonoBehaviour, ITextMeshVertexEffect
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

        [SerializeField]
        private Vector3 startScale = Vector3.zero;
        
        [SerializeField]
        protected Vector3 endScale = Vector3.one;
        
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
                effectManager.RemoveVertexEffect(this);
                StopCoroutine(coroutine);
                onComplete.Invoke();
            }
            coroutine = null;
        }
        
        private IEnumerator RunAnimation()
        {
            effectManager.AddVertexEffect(this);
            
            progress = 0;
            while (progress < currentMessage.Length)
            {
                progress += Time.smoothDeltaTime * charactersPerSecond;
                yield return null;
            }
            
            Finish();
        }

        public void UpdateVertexEffect(TMP_CharacterInfo charInfo, ref EffectData data)
        {
            var t = Mathf.Clamp01(progress - charInfo.index);
            data.localScale = Vector3.Lerp(startScale, endScale, EaseInQuad(0,1,t) );
        }
        
        public static float EaseInSine(float start, float end, float value)
        {
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }
        
        public static float EaseInQuad(float start, float end, float value)
        {
            end -= start;
            return end * value * value + start;
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


