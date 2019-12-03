using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameframe.GUI.PanelSystem
{
    internal enum PanelViewControllerState
    {
        Disappeared,
        Appearing,
        Appeared,
        Disappearing
    }
    
    internal sealed class PanelViewControllerBase : IPanelViewController
    {
        private readonly PanelType panelType = null;

        private PanelViewBase panelView = null;

        private PanelViewControllerState state = PanelViewControllerState.Disappeared;
        
        private CancellationTokenSource cancellationTokenSource = null;

        private IPanelViewContainer parentPanelViewContainer = null;
        
        public PanelType PanelType => panelType;
        public PanelViewBase View => panelView;
        public bool IsViewLoaded => panelView != null;

        public IPanelViewContainer ParentViewContainer => parentPanelViewContainer;
        
        private readonly Action didLoad = null;
        private readonly Action willAppear = null;
        private readonly Action didAppear = null;
        private readonly Action willDisappear = null;
        private readonly Action didDisappear = null;
        
        public PanelViewControllerBase(PanelType type, Action didLoad, Action willAppear, Action didAppear, Action willDisappear, Action didDisappear)
        {
            panelType = type;
            this.didLoad = didLoad;
            this.willAppear = willAppear;
            this.didAppear = didAppear;
            this.willDisappear = willDisappear;
            this.didDisappear = didDisappear;
        }
        
        public PanelViewControllerBase(PanelType type, PanelViewBase view, Action didLoad, Action willAppear, Action didAppear, Action willDisappear, Action didDisappear)
        {
            panelType = type;
            panelView = view;
            this.didLoad = didLoad;
            this.willAppear = willAppear;
            this.didAppear = didAppear;
            this.willDisappear = willDisappear;
            this.didDisappear = didDisappear;
        }

        public void SetParentViewContainer(IPanelViewContainer parent)
        {
            parentPanelViewContainer = parent;
            if (panelView != null)
            {
                panelView.transform.SetParent(parentPanelViewContainer.ParentTransform);
            }
        }

        public async Task LoadViewAsync()
        {
            if (IsViewLoaded)
            {
                return;
            }
            
            var prefab = await panelType.GetPrefabAsync();
            bool cachedState = prefab.gameObject.activeSelf;
            prefab.gameObject.SetActive(false);
            panelView = Object.Instantiate(prefab,ParentViewContainer?.ParentTransform);
            prefab.gameObject.SetActive(cachedState);
            didLoad?.Invoke();
        }
        
        public async Task ShowAsync()
        {
            //If we're currently appeared or appearing we're already doing the thing we wanna be doing so just return
            if (state == PanelViewControllerState.Appeared || state == PanelViewControllerState.Appearing)
            {
                return;
            }
            
            //If we're in the middle of disappearing we need to cancel the disappearing action
            if (state == PanelViewControllerState.Disappearing)
            {
                //Cancel Current Transition
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
            }
            
            state = PanelViewControllerState.Appearing;

            if (cancellationTokenSource == null)
            {
                cancellationTokenSource = new CancellationTokenSource();
            }

            var currentToken = cancellationTokenSource.Token;

            if (!IsViewLoaded)
            {
                await LoadViewAsync();

                if (currentToken.IsCancellationRequested)
                {
                    return;
                }
            }

            willAppear?.Invoke();
            
            await panelView.ShowAsync(currentToken);

            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }
            
            state = PanelViewControllerState.Appeared;
            
            didAppear?.Invoke();
        }
        
        public async Task HideAsync()
        {
            //If we're already disappeared or disappearing we're already doing the right thing so just return
            if (state == PanelViewControllerState.Disappeared || state == PanelViewControllerState.Disappearing)
            {
                return;
            }
            
            //If we're currently appearing we should cancel the appear using our cancellation token
            if (state == PanelViewControllerState.Appearing && cancellationTokenSource != null)
            {
                //Cancel Current Transition
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
            }

            state = PanelViewControllerState.Disappearing;

            if (cancellationTokenSource == null)
            {
                cancellationTokenSource = new CancellationTokenSource();
            }

            var currentToken = cancellationTokenSource.Token;
            
            willDisappear?.Invoke();

            await panelView.HideAsync(currentToken);

            if (currentToken.IsCancellationRequested)
            {
                return;
            }
            
            didDisappear?.Invoke();
            
            state = PanelViewControllerState.Disappeared;
        }

    }

}