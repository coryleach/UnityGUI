using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameframe.GUI.Utility;
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

        private List<PanelViewController> activeControllers = null;
        
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
            if (activeControllers == null)
            {
                activeControllers = ListPool<PanelViewController>.Get();
            }
            
            var visiblePushPanelOptions = ListPool<PushPanelOptions>.Get();
            var showControllers = ListPool<PanelViewController>.Get();
            
            PanelStackUtility.GetVisiblePanels(panelStackSystem,visiblePushPanelOptions);
            PanelStackUtility.GetControllersForOptions(panelControllers,visiblePushPanelOptions,showControllers);
            
            var hideControllers = activeControllers.Where(x => !showControllers.Contains(x));
            
            //Simultaneous Show/Hide
            //TODO: Support Instant, ShowThenHide, HideThenShow, etc
            var transitionTask = TransitionDefault(hideControllers, showControllers);
            
            ListPool<PanelViewController>.Release(activeControllers);
            activeControllers = showControllers;
            
            ListPool<PushPanelOptions>.Release(visiblePushPanelOptions);

            await transitionTask;
        }

        private static async Task TransitionDefault(IEnumerable<PanelViewController> hideControllers, IEnumerable<PanelViewController> showControllers)
        {
            var hideTasks = hideControllers.Select(x => x.HideAsync());
            var showTasks = showControllers.Select(x => x.ShowAsync());
            await Task.WhenAll(hideTasks);
            await Task.WhenAll(showTasks);
        }

#if UNITY_EDITOR
        private void OnValidate()
        { 
           panelControllers = GetComponentsInChildren<PanelViewController>().ToList();
        }
#endif
        
    }

    
    
}

