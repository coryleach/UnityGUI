using System;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem 
{
    public class PanelViewControllerRegisterer : MonoBehaviour
    {
        [SerializeField] 
        private PanelViewControllerProvider provider = null;

        private IPanelViewController controller = null;
        
        private void Start()
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
