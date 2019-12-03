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
        
        private List<IPanelViewController> activeControllers = null;

        public RectTransform ParentTransform => (RectTransform)transform;

        [ContextMenu("GC Collect")]
        public void GCCollect()
        {
            GC.Collect(0, GCCollectionMode.Forced);
        }
        
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
            
            //Simultaneous Show/Hide
            //TODO: Support Instant, ShowThenHide, HideThenShow, etc
            var transitionTask = TransitionDefault(hideControllers, showControllers);
            
            ListPool<IPanelViewController>.Release(activeControllers);
            activeControllers = showControllers;

            try
            {
                if (eventManager != null)
                {
                    eventManager.Lock();
                }
                
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

        private async Task TransitionDefault(IEnumerable<IPanelViewController> hideControllers, IEnumerable<IPanelViewController> showControllers)
        {
            var hideTasks = hideControllers.Select(x => x.HideAsync());
            
            var showTasks = showControllers.Select(x =>
            {
                x.SetParentViewContainer(this);
                return x.ShowAsync();
            });
            
            await Task.WhenAll(hideTasks);
            await Task.WhenAll(showTasks);
        }

    }
    
}

