using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.PanelSystem
{
    public class OkCancelPanelPusher : MonoBehaviour
    {
        public PanelType panelType;
        public PanelStackSystem panelStackSystem;
        
        public UnityEvent OnConfirm = new UnityEvent();
        public UnityEvent OnCancel = new UnityEvent();

        [ContextMenu("Push")]
        public void Push()
        {
            var controller = new OkCancelPanelViewController(panelType, Callback);
            panelStackSystem.Push(controller);
        }

        private void Callback(bool ok)
        {
            if (ok)
            {
                OnConfirm.Invoke();
            }
            else
            {
                OnCancel.Invoke();
            }
        }
    }
}

