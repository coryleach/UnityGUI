using System;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class OkCancelPanelViewController : PanelViewController
    {
        private readonly Action<bool> callback;

        public OkCancelPanelViewController(PanelType type, Action<bool> callback) : base(type)
        {
            this.callback = callback;
        }

        protected override void ViewWillAppear(ITransitionEvent transitionEvent)
        {
            base.ViewWillAppear(transitionEvent);
            SubscribeToView();
        }

        protected override void ViewWillDisappear(ITransitionEvent transitionEvent)
        {
            base.ViewWillDisappear(transitionEvent);
            UnsubscribeToView();
        }

        private void SubscribeToView()
        {
            if (this.View is OkCancelPanelView okCancelView)
            {
                okCancelView.onConfirm += Ok;
                okCancelView.onCancel += Cancel;
            }
            else
            {
                Debug.LogError($"OkCancelPanelViewController tried to subscribe to its view but view was the wrong type. View = {View.GetType()}");
            }
        }

        private void UnsubscribeToView()
        {
            if (this.View is OkCancelPanelView okCancelView)
            {
                okCancelView.onConfirm -= Ok;
                okCancelView.onCancel -= Cancel;
            }
            else
            {
                Debug.LogError($"OkCancelPanelViewController tried to subscribe to its view but view was the wrong type. View = {View.GetType()}");
            }
        }

        protected virtual void Ok()
        {
            PopStack();
            callback?.Invoke(true);
        }

        protected virtual void Cancel()
        {
            PopStack();
            callback?.Invoke(false);
        }

        protected virtual void PopStack()
        {
            if (ParentViewContainer is PanelStackControllerBehaviour stackController)
            {
                stackController.System.Pop();
            }
            else
            {
                Debug.LogError($"ParentViewContainer is not a stack controller. ParentViewContainer = {ParentViewContainer?.GetType()}");
            }
            UnsubscribeToView();
        }
    }
}
