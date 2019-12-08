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
    /// PanelStackController
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class PanelStackController : MonoBehaviour, IPanelStackController, IPanelViewContainer
    {
        [SerializeField] 
        private UIEventManager eventManager;
        
        [SerializeField] 
        private PanelStackSystem panelStackSystem = null;

        public PanelStackSystem System => panelStackSystem;
        
        private List<IPanelViewController> activeControllers = null;

        public RectTransform ParentTransform => (RectTransform)transform;
        
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
                activeControllers = ListPool<IPanelViewController>.Get();
            }
            
            var showControllers = ListPool<IPanelViewController>.Get();
            PanelStackUtility.GetVisiblePanelViewControllers(panelStackSystem,showControllers);
            
            var hideControllers = activeControllers.Where(x => !showControllers.Contains(x));
            

            try
            {
                if (eventManager != null)
                {
                    eventManager.Lock();
                }

                //Load Views
                await LoadViews(showControllers);
                
                //Sort Views so things overlay property
                SortViews();
                
                //Simultaneous Show/Hide
                //TODO: Support Instant, ShowThenHide, HideThenShow, etc
                var transitionTask = TransitionDefault(hideControllers, showControllers);
            
                ListPool<IPanelViewController>.Release(activeControllers);
                activeControllers = showControllers;
                
                await transitionTask;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                if (eventManager != null)
                {
                    eventManager.Unlock();
                }    
            }
        }

        private void SortViews()
        {
            for (int i = 0; i < panelStackSystem.Count; i++)
            {
                if (panelStackSystem[i].IsViewLoaded)
                {
                    panelStackSystem[i].View.transform.SetSiblingIndex(i);
                }
            }
        }

        private async Task LoadViews(IEnumerable<IPanelViewController> controllers)
        {
            var tasks = ListPool<Task>.Get();
            
            foreach (var controller in controllers)
            {
                if (controller.IsViewLoaded)
                {
                    continue;
                }
                
                controller.SetParentViewContainer(this);
                tasks.Add(controller.LoadViewAsync());
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
            
            ListPool<Task>.Release(tasks);
        }
        
        private async Task TransitionDefault(IEnumerable<IPanelViewController> hideControllers, IEnumerable<IPanelViewController> showControllers)
        {
            var hideTasks = ListPool<Task>.Get();
            foreach (var controller in hideControllers)
            {
                hideTasks.Add(controller.HideAsync());
            }

            var showTasks = ListPool<Task>.Get();
            foreach (var controller in showControllers)
            {
                controller.SetParentViewContainer(this);
                showTasks.Add(controller.ShowAsync());
            }

            await Task.WhenAll(hideTasks);
            await Task.WhenAll(showTasks);
            
            ListPool<Task>.Release(hideTasks);
            ListPool<Task>.Release(showTasks);
        }

    }
    
}

