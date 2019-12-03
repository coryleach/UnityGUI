using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// MonoBehaviour implementing IPanelViewController interface
    /// internally this is just a humble object wrapper for PanelViewControllerBase which does the heavy lifting
    /// </summary>
    public class PanelViewControllerBehaviour : MonoBehaviour, IPanelViewController
    {
        [SerializeField]
        private PanelType panelType = null;

        [SerializeField]
        private PanelViewBase panelView = null;
        
        private PanelViewControllerBase baseController = null;

        private PanelViewControllerBase BaseController =>
            baseController ?? (baseController = CreateController());

        protected virtual void Awake()
        {
            //Base controller is created 
            //Create the base controller in awake if it hasn't been created yet
            if (baseController == null)
            {
                baseController = CreateController();
            }
        }

        private PanelViewControllerBase CreateController()
        {
            return new PanelViewControllerBase(panelType,panelView, 
                ViewDidLoad,
                ViewWillAppear, 
                ViewDidAppear, 
                ViewWillDisappear, 
                ViewDidDisappear);
        }
        
        public PanelType PanelType => BaseController.PanelType;
        
        public Task LoadViewAsync() => BaseController.LoadViewAsync();

        public bool IsViewLoaded => BaseController.IsViewLoaded;

        public async void Show()
        {
            await ShowAsync();
        }

        public Task ShowAsync() => BaseController.ShowAsync();

        public async void Hide()
        {
            await HideAsync();
        }

        public Task HideAsync() => BaseController.HideAsync();
        
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
            if (panelType != null)
            {
                name = $"PanelController - {panelType.name}";
            }
        }
        #endif
        
    }
}


