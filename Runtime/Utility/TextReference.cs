using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameframe.GUI.Utility
{
    [Serializable]
    public class TextReference
    {
        [SerializeField]
        private Text uguiText;
        
        [SerializeField]
        private TextMeshProUGUI tmpText;

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