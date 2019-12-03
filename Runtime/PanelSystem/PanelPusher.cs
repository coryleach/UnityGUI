using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelPusher : MonoBehaviour
    {
        [SerializeField] private PanelStackSystem stack;
        
        [SerializeField] private PanelViewControllerProvider provider;

        [SerializeField] private PanelType panelType;

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
    } 
}

