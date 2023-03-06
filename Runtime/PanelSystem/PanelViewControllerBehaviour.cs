using System.Threading.Tasks;
using Gameframe.GUI.Extensions;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public abstract class PanelViewControllerBehaviourBase<TPanelViewBase> : MonoBehaviour, IPanelViewController, IPanelViewContainer
    {
        [SerializeField]
        protected PanelType panelType;

        [SerializeField, Help("When PanelView is null the prefab assigned to the Panel Type will be instantiated and used at runtime")]
        protected PanelViewBase panelView;

        private PanelViewControllerBase baseController;

        internal PanelViewControllerBase BaseController => baseController ?? (baseController = CreateController());

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
                ViewDidUnload,
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

        public abstract TPanelViewBase View { get; }

        public Task LoadViewAsync() => BaseController.LoadViewAsync();
        public Task UnloadViewAsync() => BaseController.UnloadViewAsync();

        public bool IsViewLoaded => BaseController.IsViewLoaded;

        public IPanelViewContainer ParentViewContainer => BaseController.ParentViewContainer;

        private RectTransform rectTransform;

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

        public async void Show(ITransitionEvent transitionEvent = null)
        {
            await ShowAsync(transitionEvent).ConfigureAwait(false);
        }

        public async void Hide(ITransitionEvent transitionEvent = null)
        {
            await HideAsync(transitionEvent).ConfigureAwait(false);
        }

        public async void Show(bool immediate, ITransitionEvent transitionEvent = null)
        {
            await ShowAsync(immediate, transitionEvent).ConfigureAwait(false);
        }

        public Task ShowAsync(bool immediate, ITransitionEvent transitionEvent = null) => BaseController.ShowAsync(immediate, transitionEvent);

        public Task ShowAsync(ITransitionEvent transitionEvent = null) => BaseController.ShowAsync(false, transitionEvent);

        public async void Hide(bool immediate, ITransitionEvent transitionEvent = null)
        {
            await HideAsync(immediate, transitionEvent).ConfigureAwait(false);
        }

        public Task HideAsync(bool immediate, ITransitionEvent transitionEvent = null) => BaseController.HideAsync(immediate, transitionEvent);

        public Task HideAsync(ITransitionEvent transitionEvent = null) => BaseController.HideAsync(false, transitionEvent);

        protected virtual void ViewDidLoad()
        {
        }

        protected virtual void ViewDidUnload()
        {
        }

        protected virtual void ViewWillAppear(ITransitionEvent transitionEvent)
        {
        }

        protected virtual void ViewDidAppear(ITransitionEvent transitionEvent)
        {
        }

        protected virtual void ViewWillDisappear(ITransitionEvent transitionEvent)
        {
        }

        protected virtual void ViewDidDisappear(ITransitionEvent transitionEvent)
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
    public abstract class PanelViewControllerBehaviour<TPanelViewBase> : PanelViewControllerBehaviourBase<TPanelViewBase> where TPanelViewBase : PanelViewBase
    {
        public override TPanelViewBase View => (TPanelViewBase)BaseController.View;
    }

    /// <summary>
    /// MonoBehaviour implementing IPanelViewController interface
    /// internally this is just a humble object wrapper for PanelViewControllerBase which does the heavy lifting
    /// </summary>
    public class PanelViewControllerBehaviour : PanelViewControllerBehaviourBase<PanelViewBase>
    {
        public override PanelViewBase View { get; }
    }
}
