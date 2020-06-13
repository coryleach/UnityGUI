using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{

    public abstract class PanelViewControllerBase<TPanelView> : IPanelViewController where TPanelView : PanelViewBase
    {
        private readonly PanelViewControllerBase baseController;

        protected PanelViewControllerBase(PanelType type)
        {
            baseController = new PanelViewControllerBase(type, ViewDidLoad,ViewWillAppear,ViewDidAppear,ViewWillDisappear,ViewDidDisappear);
        }

        public PanelViewControllerState State => baseController.State;
        public PanelType PanelType => baseController.PanelType;

        PanelViewBase IPanelViewController.View => baseController.View;

        public TPanelView View => (TPanelView)baseController.View;
        
        public bool IsViewLoaded => baseController.IsViewLoaded;

        public Task LoadViewAsync() => baseController.LoadViewAsync();

        public Task HideAsync(bool immediate = false) => baseController.HideAsync(immediate);

        public Task ShowAsync(bool immediate = false) => baseController.ShowAsync(immediate);

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
        
    }
    
    /// <summary>
    /// PanelViewController
    /// Can be subclassed to provide custom controller functionality via the methods:
    /// ViewDidLoad,
    /// ViewWillAppear,
    /// ViewDidAppear,
    /// ViewWillDisappear,
    /// ViewDidDisappear
    /// </summary>
    public class PanelViewController : PanelViewControllerBase<PanelViewBase>
    {
        internal static SynchronizationContext MainSyncContext { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            MainSyncContext = SynchronizationContext.Current;
        }

        internal static void CleanupDestroyedPanel(object state)
        {
            //If we're not running we don't need to do anything
            if (!Application.isPlaying)
            {
                return;
            }
            //If we have a panel view and we're running we should be able to destroy it
            //This is so that we could allocate controllers on the fly
            //when they're potenitally popped from a stack and cleaned up 
            //they will then clean up the views they created as well.
            var view = state as PanelViewBase;
            if (view != null)
            {
                Object.Destroy(view.gameObject);
            }
        }
        
        /// <summary>
        /// This destructor will destroy the panel view that it may have instantiated
        /// </summary>
        ~PanelViewController()
        {
            MainSyncContext?.Post(CleanupDestroyedPanel,View);
        }

        public PanelViewController(PanelType type) : base(type)
        {
        }
    }
    
    public class PanelViewController<TPanelView> : PanelViewControllerBase<TPanelView> where TPanelView : PanelViewBase
    {
        public PanelViewController(PanelType type) : base(type)
        {
        }
        
        /// <summary>
        /// This destructor will destroy the panel view that it may have instantiated
        /// </summary>
        ~PanelViewController()
        {
            //Using internal variable & method from PanelViewController to avoid AOT compile issues with this generic class
            PanelViewController.MainSyncContext?.Post(PanelViewController.CleanupDestroyedPanel,View);
        }
    }

}
