using System.Threading.Tasks;

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
            baseController = new PanelViewControllerBase(type,ViewDidLoad,ViewWillAppear,ViewDidAppear,ViewWillDisappear,ViewDidDisappear);
        }

        public PanelType PanelType => baseController.PanelType;

        public bool IsViewLoaded => baseController.IsViewLoaded;

        public Task LoadViewAsync() => baseController.LoadViewAsync();

        public Task HideAsync() => baseController.HideAsync();

        public Task ShowAsync() => baseController.ShowAsync();
        
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
}
