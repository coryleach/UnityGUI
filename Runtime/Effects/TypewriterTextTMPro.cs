using System.Collections;
using TMPro;
using UnityEngine;

namespace Gameframe.GUI
{
    public class TypewriterTextTMPro : BaseTypewriterText
    {
        [SerializeField]
        private TextMeshProUGUI text = null;

        public override int CharacterPerSecond
        {
            get => charactersPerSecond;
            set
            {
                charactersPerSecond = value;
                waitInterval = new WaitForSeconds(1.0f / charactersPerSecond);
            }
        }

        [ContextMenu("Play")]
        public override void Play()
        {
            Play(text.text);
        }

        public override void Finish()
        {
            base.Finish();
            text.text = currentMessage;
        }

        private WaitForEndOfFrame waitEndOfFrame = new WaitForEndOfFrame();
        private WaitForSeconds waitInterval = null;
        
        protected override IEnumerator RunType()
        {
            if (waitInterval == null)
            {
                waitInterval = new WaitForSeconds(1.0f / charactersPerSecond);
            }
            
            text.text = currentMessage;
            text.maxVisibleCharacters = 0;
            
            for (var i = 0; i < currentMessage.Length; i++)
            {
                text.maxVisibleCharacters = i;
                yield return waitInterval;
                yield return waitEndOfFrame;
            }
            
            text.maxVisibleCharacters = 99999;
            typeCoroutine = null;
            onComplete.Invoke();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
            }
            waitInterval = new WaitForSeconds(1.0f/charactersPerSecond);
        }
    }
}