using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.Utility;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelSwapController : IPanelSwapController
    {
        private readonly IUIEventManager eventManager = null;
        private readonly IPanelSwapSystem panelSwapSystem = null;
        private readonly IPanelViewContainer container = null;
        
        private IPanelViewController activePanelController = null;
        
        public PanelSwapController(IPanelSwapSystem swapSystem, IPanelViewContainer viewContainer, IUIEventManager eventManager = null)
        {
            panelSwapSystem = swapSystem;
            container = viewContainer;
            this.eventManager = eventManager;
        }
        
        public async Task TransitionAsync()
        {
            try
            {
                //Not using null propagation because this would bypass the unity null check
                // ReSharper disable once UseNullPropagation
                if (null != eventManager)
                {
                    eventManager.Lock();
                }
                
                var hideController = activePanelController;
                var showController = panelSwapSystem.CurrentViewController;
                
                //Load Views
                if (showController != null)
                {
                    await LoadView(showController);
                }
                
                var transitionTask = TransitionDefault(hideController, showController);
                await transitionTask;
                
                activePanelController = showController;
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
        
        private async Task LoadView(IPanelViewController controller)
        {
            if (controller.IsViewLoaded)
            {
                return;
            }
            controller.SetParentViewContainer(container);
            await controller.LoadViewAsync();
        }
        
        private async Task TransitionDefault(IPanelViewController hideController, IPanelViewController showController)
        {
            Task hideTask = null;
            Task showTask = null;

            if (hideController != null)
            {
                hideTask = hideController.HideAsync();
            }
            
            if (showController != null)
            {
                showController.SetParentViewContainer(container);
                showTask = showController.ShowAsync();
            }

            if (hideTask != null)
            {
                await hideTask;
            }

            if (showTask != null)
            {
                await showTask;
            }
        }

    }
}


