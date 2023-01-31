using System;
using System.Threading;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Gameframe.GUI.PanelSystem
{
    internal sealed class PanelViewControllerBase : IPanelViewController, IDisposable
    {
        private readonly PanelType panelType;

        private PanelViewBase panelView;

        private PanelViewControllerState state = PanelViewControllerState.Disappeared;
        public PanelViewControllerState State => state;

        private CancellationTokenSource cancellationTokenSource;

        private IPanelViewContainer parentPanelViewContainer;

        public PanelType PanelType => panelType;
        public PanelViewBase View => panelView;
        public bool IsViewLoaded => panelView != null;

        public IPanelViewContainer ParentViewContainer => parentPanelViewContainer;

        private readonly Action didLoad;
        private readonly Action willAppear;
        private readonly Action didAppear;
        private readonly Action willDisappear;
        private readonly Action didDisappear;

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

            if (prefab == null)
            {
                throw new ArgumentNullException($"PanelType {panelType.name} returned null panel prefab.");
            }

            bool cachedState = prefab.gameObject.activeSelf;
            prefab.gameObject.SetActive(false);
            panelView = Object.Instantiate(prefab,ParentViewContainer?.ParentTransform);
            prefab.gameObject.SetActive(cachedState);
            didLoad?.Invoke();
        }

        public async Task ShowAsync(bool immediate = false, ITransitionEvent transitionEvent = null)
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
                await LoadViewAsync().ConfigureAwait(true);

                if (currentToken.IsCancellationRequested)
                {
                    return;
                }
            }

            willAppear?.Invoke();

            if (immediate)
            {
                panelView.ShowImmediate();
            }
            else
            {
                await panelView.ShowAsync(currentToken);
            }

            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            state = PanelViewControllerState.Appeared;

            didAppear?.Invoke();
        }

        public async Task HideAsync(bool immediate = false, ITransitionEvent transitionEvent = null)
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

            if (immediate)
            {
                panelView.HideImmediate();
            }
            else
            {
                await panelView.HideAsync(currentToken);
            }

            if (currentToken.IsCancellationRequested)
            {
                return;
            }

            didDisappear?.Invoke();

            state = PanelViewControllerState.Disappeared;
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }
    }

}
