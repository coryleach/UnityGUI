using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelPusher : MonoBehaviour
    {
        [SerializeField] private PanelStackSystem stack = null;
        
        [SerializeField] private PanelViewControllerProvider provider = null;

        [SerializeField] private PanelType panelType = null;

        public void Push()
        {
            IPanelViewController controller;

            if (provider != null)
            {
                controller = provider.GetOrCreate(panelType);
            }
            else
            {
                controller = new PanelViewController(panelType);    
            }

            stack.Push(controller);
        }
        
        public void ClearAndPush()
        {
            IPanelViewController controller;

            if (provider != null)
            {
                controller = provider.GetOrCreate(panelType);
            }
            else
            {
                controller = new PanelViewController(panelType);    
            }

            stack.ClearAndPush(controller);
        }
        
    } 
}

