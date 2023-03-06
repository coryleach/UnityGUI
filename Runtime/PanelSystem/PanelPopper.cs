using UnityEngine;
using UnityEngine.UI;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelPopper : MonoBehaviour
    {

        [SerializeField]
        private ScriptablePanelStackSystem stack;

        [SerializeField]
        private PopEvent popEvent = PopEvent.Manual;

        public enum PopEvent
        {
            Manual,
            ButtonClick,
            Awake,
            Start,
            Enable
        }

        private void Awake()
        {
            if (popEvent == PopEvent.Awake)
            {
                Pop();
            }
            else if (popEvent == PopEvent.ButtonClick)
            {
                GetComponent<Button>()?.onClick.AddListener(Pop);
            }
        }

        private void Start()
        {
            if (popEvent == PopEvent.Start)
            {
                Pop();
            }
        }

        private void OnEnable()
        {
            if (popEvent == PopEvent.Enable)
            {
                Pop();
            }
        }

        public void Pop()
        {
            stack.Pop();
        }
    }
}
