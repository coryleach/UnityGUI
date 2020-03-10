using System.Collections;
using Gameframe.GUI.Tween;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class MoveInText : MonoBehaviour, ITextMeshVertexEffect
    {
        [SerializeField] 
        private TextMeshEffectTMPro effectManager;
        
        [SerializeField] 
        protected bool playOnEnable = false;

        [SerializeField] 
        private Easing easeType = Easing.Linear;
        
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
        private Vector3 startOffset = Vector3.zero;
        
        [SerializeField]
        protected Vector3 endOffset = Vector3.one;
        
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
            t = EaseFunctions.Ease(easeType, t);
            var delta = endOffset - startOffset;
            data.localPosition = startOffset + delta * t;
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


