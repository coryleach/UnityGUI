using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// PanelViewController
    /// Can be subclassed to provide custom controller functionality via the methods:
    /// ViewDidLoad,
    /// ViewWillAppear,
    /// ViewDidAppear,
    /// ViewWillDisappear,
    /// ViewDidDisappear
    /// </summary>
    public class PanelViewController : IPanelViewController
    {
        private readonly PanelViewControllerBase baseController;
        
        public PanelViewController(PanelType type)
        {
            baseController = new PanelViewControllerBase(type, ViewDidLoad,ViewWillAppear,ViewDidAppear,ViewWillDisappear,ViewDidDisappear);
        }

        public PanelType PanelType => baseController.PanelType;

        public PanelViewBase View => baseController.View;
        
        public bool IsViewLoaded => baseController.IsViewLoaded;

        public Task LoadViewAsync() => baseController.LoadViewAsync();

        public Task HideAsync() => baseController.HideAsync();

        public Task ShowAsync() => baseController.ShowAsync();

        public IPanelViewContainer ParentViewContainer => baseController.ParentViewContainer;
        
        public void SetParentViewContainer(IPanelViewContainer parent) => baseController.SetParentViewContainer(parent);

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

        private static SynchronizationContext mainSyncContext = null;
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            mainSyncContext = SynchronizationContext.Current;
        }
        
        ~PanelViewController()
        {
            mainSyncContext?.Post((state) =>
            {
                if (!UnityEngine.Application.isPlaying)
                {
                    return;
                }
                var view = state as PanelViewBase;
                if (view != null)
                {
                    UnityEngine.Object.Destroy(view.gameObject);
                }
            },baseController.View);
        }
        
    }
}
