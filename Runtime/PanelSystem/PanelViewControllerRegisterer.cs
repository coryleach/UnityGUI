using System;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem 
{
    public class PanelViewControllerRegisterer : MonoBehaviour
    {
        [SerializeField, Help("This component registers a panel controller with a panel controller provider. The panel provider is used to map PanelType to PanelViewController.")] 
        private PanelViewControllerProvider provider = null;

        private IPanelViewController controller = null;

        [SerializeField]
        private RegisterEvent regsiterOnEvent = RegisterEvent.Start; 
        
        public enum RegisterEvent
        {
            Awake,
            Start
        }
        
        private void Awake()
        {
            if (regsiterOnEvent == RegisterEvent.Awake)
            {
                Register();
            }
        }
        
        private void Start()
        {
            if (regsiterOnEvent == RegisterEvent.Start)
            {
                Register();
            }
        }

        private void Register()
        {
            controller = GetComponent<IPanelViewController>();
            if (controller != null)
            {
                provider.Add(controller);
            }   
        }

        private void OnDestroy()
        {
            if (controller != null)
            {
                provider.Remove(controller);
            }
        }
    }
}
