using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Gameframe.GUI.PanelSystem
{
    public class OkCancelPanelPusher : MonoBehaviour
    {
        [SerializeField] private PanelType panelType;
        [SerializeField] private ScriptablePanelStackSystem panelStackSystem;
        
        [FormerlySerializedAs("OnConfirm"), SerializeField] 
        private UnityEvent onConfirm = new UnityEvent();
        public UnityEvent OnConfirm => onConfirm;
        
        [FormerlySerializedAs("OnCancel"), SerializeField] 
        private UnityEvent onCancel = new UnityEvent();
        public UnityEvent OnCancel => onCancel;
        
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
                onConfirm.Invoke();
            }
            else
            {
                onCancel.Invoke();
            }
        }
    }
}

