using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.Utility;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{

    /// <summary>
    /// PanelStackControllerBehaviour is a MonoBehaviour wrapper around a PanelStackController instance
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class PanelStackControllerBehaviour : MonoBehaviour, IPanelStackController, IPanelViewContainer
    {
        [SerializeField] 
        private UIEventManager eventManager = null;
        
        [SerializeField] 
        private PanelStackSystem panelStackSystem = null;

        public PanelStackSystem System => panelStackSystem;
        
        public RectTransform ParentTransform => (RectTransform)transform;

        private PanelStackController baseController = null;

        private PanelStackController BaseController =>
            baseController ??
            (baseController = new PanelStackController(panelStackSystem, this, eventManager));

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
            await BaseController.TransitionAsync();
        }
    }
    
}

