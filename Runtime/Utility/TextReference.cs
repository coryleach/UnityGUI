using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Utility
{
    [Serializable]
    public class TextReference
    {
        [SerializeField]
        private Text uguiText = null;
        
        [SerializeField]
        private TextMeshProUGUI tmpText = null;

        public string Text
        {
            get
            {
                if (tmpText != null)
                {
                    return tmpText.text;
                }
                return uguiText != null ? uguiText.text : string.Empty;
            }
            set
            {
                if (tmpText != null)
                {
                    tmpText.text = value;
                }
                if (uguiText != null)
                {
                    uguiText.text = value;
                }
            }
        }
        
        public Color Color
        {
            get
            {
                if (tmpText != null)
                {
                    return tmpText.color;
                }
                return uguiText != null ? uguiText.color : default(Color);
            }
            set
            {
                if (tmpText != null)
                {
                    tmpText.color = value;
                }
                if (uguiText != null)
                {
                    uguiText.color = value;
                }
            }
        }
    }
}