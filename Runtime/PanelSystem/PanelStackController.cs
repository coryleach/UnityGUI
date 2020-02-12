using System;
using System.Collections.Generic;
using System.Linq;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.Utility;
using UnityEngine;
using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelStackController : IPanelStackController
    {
        private List<IPanelViewController> activeControllers = new List<IPanelViewController>();
        private readonly IUIEventManager eventManager = null;
        private readonly IPanelStackSystem panelStackSystem = null;
        private readonly IPanelViewContainer container = null;
        
        public PanelStackController(IPanelStackSystem stackSystem, IPanelViewContainer viewContainer, IUIEventManager eventManager = null)
        {
            panelStackSystem = stackSystem;
            container = viewContainer;
            this.eventManager = eventManager;
        }

        public async Task TransitionAsync()
        {
            if (activeControllers == null)
            {
                activeControllers = ListPool<IPanelViewController>.Get();
            }
            
            var showControllers = ListPool<IPanelViewController>.Get();
            
            try
            {
                PanelStackUtility.GetVisiblePanelViewControllers(panelStackSystem,showControllers);
            }
            catch (Exception e)
            {
                throw new AggregateException("Unable to get visible panel view controllers.",e);
            }
            
            var hideControllers = activeControllers.Where(x => !showControllers.Contains(x));

            try
            {
                //Not using null propagation because this would bypass the unity null check
                // ReSharper disable once UseNullPropagation
                if (null != eventManager)
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
                //Not using null propagation because this would bypass the unity null check
                // ReSharper disable once UseNullPropagation
                if (null != eventManager)
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
                
                controller.SetParentViewContainer(container);
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
                controller.SetParentViewContainer(container);
                showTasks.Add(controller.ShowAsync());
            }

            await Task.WhenAll(hideTasks);
            await Task.WhenAll(showTasks);
            
            ListPool<Task>.Release(hideTasks);
            ListPool<Task>.Release(showTasks);
        }
    }
    
}
