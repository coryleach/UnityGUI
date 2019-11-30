using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// PanelStackContainerController
    /// </summary>
    public class PanelStackController : MonoBehaviour, IPanelStackController
    {
        [SerializeField] 
        private PanelStackSystem panelStackSystem = null;
        
        [SerializeField]
        private List<PanelViewController> panelControllers = new List<PanelViewController>();
        
        private void OnEnable()
        {
            panelStackSystem.AddController(this);
        }
        
        private void OnDisable()
        {
            panelStackSystem.RemoveController(this);
        }
        
        public async Task TransitionAsync()
        {
            await Task.Delay(1000);
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        { 
           panelControllers = GetComponentsInChildren<PanelViewController>().ToList();
        }
        #endif
        
    }    
}

