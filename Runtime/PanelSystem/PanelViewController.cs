using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelViewController : MonoBehaviour
    {
        private enum PanelViewControllerState
        {
            Disappeared,
            Appearing,
            Appeared,
            Disappearing
        }
        
        [SerializeField]
        private PanelType panelType = null;
        
        [SerializeField]
        private PanelViewBase panelView = null;
        
        private PanelViewControllerState state = PanelViewControllerState.Disappeared;
        
        private CancellationTokenSource cancellationTokenSource = null;

        public async Task LoadViewAsync()
        {
            if (IsViewLoaded)
            {
                return;
            }
            
            var prefab = await panelType.GetPrefabAsync();
            prefab.gameObject.SetActive(false);
            panelView = Instantiate(prefab, transform);
            ViewDidLoad();
        }

        public bool IsViewLoaded => panelView != null;

        [ContextMenu("Show")]
        public async void Show()
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

            ViewWillAppear();
            
            await panelView.ShowAsync(currentToken);

            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }
            
            state = PanelViewControllerState.Appeared;
            
            ViewDidAppear();
        }

        [ContextMenu("Hide")]
        public async void Hide()
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
            
            ViewWillDisappear();

            await panelView.HideAsync(currentToken);

            if (currentToken.IsCancellationRequested)
            {
                return;
            }
            
            ViewDidDisappear();
            
            state = PanelViewControllerState.Disappeared;
        }
        
        protected virtual void ViewDidLoad()
        {
        }
        
        protected virtual void ViewWillAppear()
        {
        }

        protected virtual void ViewDidAppear()
        {
        }

        protected virtual void ViewWillDisappear()
        {
        }

        protected virtual void ViewDidDisappear()
        {
        }

        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            name = $"PanelController - {panelType?.name}";
        }
        #endif
        
    }
}


