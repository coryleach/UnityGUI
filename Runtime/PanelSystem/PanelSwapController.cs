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
        private readonly IUIEventManager eventManager;
        private readonly IPanelSwapSystem panelSwapSystem;
        private readonly IPanelViewContainer container;
        
        private IPanelViewController activePanelController;
        
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
                    await LoadView(showController).ConfigureAwait(true);
                }
                
                var transitionTask = TransitionDefault(hideController, showController);
                await transitionTask.ConfigureAwait(true);
                
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
                await hideTask.ConfigureAwait(true);
            }

            if (showTask != null)
            {
                await showTask.ConfigureAwait(true);
            }
        }

    }
}


