using System.Threading.Tasks;
using Gameframe.GUI.Extensions;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// MonoBehaviour implementing IPanelViewController interface
    /// internally this is just a humble object wrapper for PanelViewControllerBase which does the heavy lifting
    /// </summary>
    public abstract class PanelViewControllerBehaviour<TPanelViewBase> : MonoBehaviour, IPanelViewController, IPanelViewContainer where TPanelViewBase : PanelViewBase
    {
        [SerializeField]
        private PanelType panelType = null;

        [SerializeField]
        private PanelViewBase panelView = null;
        
        private PanelViewControllerBase baseController = null;

        private PanelViewControllerBase BaseController => baseController ?? (baseController = CreateController());

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
            //Panel View always starts in the 'disappeared' state
            //If we are supplying a serialized view then we need to enforce that rule here
            if (panelView != null)
            {
                panelView.gameObject.SetActive(false);
            }
            
            var controller = new PanelViewControllerBase(panelType,panelView, 
                ViewDidLoad,
                ViewWillAppear, 
                ViewDidAppear, 
                ViewWillDisappear, 
                ViewDidDisappear);
            
            controller.SetParentViewContainer(this);

            return controller;
        }

        public PanelViewControllerState State => BaseController.State;
        
        public PanelType PanelType => BaseController.PanelType;

        PanelViewBase IPanelViewController.View => BaseController.View;

        public TPanelViewBase View => (TPanelViewBase)BaseController.View;
        
        public Task LoadViewAsync() => BaseController.LoadViewAsync();

        public bool IsViewLoaded => BaseController.IsViewLoaded;

        public IPanelViewContainer ParentViewContainer => BaseController.ParentViewContainer;

        private RectTransform rectTransform = null;

        public RectTransform ParentTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = gameObject.GetOrAddComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        public void SetParentViewContainer(IPanelViewContainer parent) => BaseController.SetParentViewContainer(parent);

        [ContextMenu("Show")]
        public async void Show()
        {
            await ShowAsync();
        }

        public Task ShowAsync(bool immediate = false) => BaseController.ShowAsync(immediate);

        [ContextMenu("Hide")]
        public async void Hide()
        {
            await HideAsync();
        }

        public Task HideAsync(bool immediate = false) => BaseController.HideAsync(immediate);
        
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
    
    /// <summary>
    /// MonoBehaviour implementing IPanelViewController interface
    /// internally this is just a humble object wrapper for PanelViewControllerBase which does the heavy lifting
    /// </summary>
    public class PanelViewControllerBehaviour : MonoBehaviour, IPanelViewController, IPanelViewContainer
    {
        [SerializeField]
        private PanelType panelType = null;

        [SerializeField]
        private PanelViewBase panelView = null;
        
        private PanelViewControllerBase baseController = null;

        private PanelViewControllerBase BaseController => baseController ?? (baseController = CreateController());

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
            //Panel View always starts in the 'disappeared' state
            //If we are supplying a serialized view then we need to enforce that rule here
            if (panelView != null)
            {
                panelView.gameObject.SetActive(false);
            }
            
            var controller = new PanelViewControllerBase(panelType,panelView, 
                ViewDidLoad,
                ViewWillAppear, 
                ViewDidAppear, 
                ViewWillDisappear, 
                ViewDidDisappear);
            
            controller.SetParentViewContainer(this);

            return controller;
        }

        public PanelViewControllerState State => BaseController.State;
        
        public PanelType PanelType => BaseController.PanelType;

        public PanelViewBase View => BaseController.View;
        
        public Task LoadViewAsync() => BaseController.LoadViewAsync();

        public bool IsViewLoaded => BaseController.IsViewLoaded;

        public IPanelViewContainer ParentViewContainer => BaseController.ParentViewContainer;

        private RectTransform rectTransform = null;

        public RectTransform ParentTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = gameObject.GetOrAddComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        public void SetParentViewContainer(IPanelViewContainer parent) => BaseController.SetParentViewContainer(parent);

        [ContextMenu("Show")]
        public async void Show()
        {
            await ShowAsync();
        }

        public Task ShowAsync(bool immediate = false) => BaseController.ShowAsync(immediate);

        [ContextMenu("Hide")]
        public async void Hide()
        {
            await HideAsync();
        }

        public Task HideAsync(bool immediate = false) => BaseController.HideAsync(immediate);
        
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


