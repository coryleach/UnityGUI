using System;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class OkCancelPanelView : AnimatedPanelView
    {
        public event Action onConfirm;
        public event Action onCancel;
        
        [ContextMenu("OK")]
        public void Ok()
        {
            onConfirm?.Invoke();
        }

        [ContextMenu("Cancel")]
        public void Cancel()
        {
            onCancel?.Invoke();
        }
    }   
}


