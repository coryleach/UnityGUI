using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public class FakePanelViewController : IPanelViewController
    {
        public PanelViewControllerState State { get; private set; } = PanelViewControllerState.Appearing;

        public PanelType PanelType { get; set; }

        public PanelViewBase View { get; private set; }

        public IPanelViewContainer ParentViewContainer { get; private set; }

        private bool loaded;

        public bool IsViewLoaded => loaded;

        public Task LoadViewAsync()
        {
            loaded = true;

            var gameObject = new GameObject();
            View = gameObject.AddComponent<PanelView>();

            return Task.CompletedTask;
        }

        public Task UnloadViewAsync()
        {
            loaded = false;
            Object.Destroy(View.gameObject);
            View = null;
            return Task.CompletedTask;
        }

        public Task HideAsync(bool immediate = false, ITransitionEvent transitionEvent = null)
        {
            State = PanelViewControllerState.Disappeared;
            return Task.CompletedTask;
        }

        public Task ShowAsync(bool immediate = false, ITransitionEvent transitionEvent = null)
        {
            State = PanelViewControllerState.Appeared;
            LoadViewAsync().Wait();
            return Task.CompletedTask;
        }

        public void SetParentViewContainer(IPanelViewContainer parent)
        {
            ParentViewContainer = parent;
        }
    }

    public class FakePanelStackController : IPanelStackController
    {
        public bool transitioned;
        public Task TransitionAsync()
        {
            transitioned = true;
            return Task.CompletedTask;
        }
    }

    public class FakePanelStackSystem : IPanelStackSystem
    {
        private readonly List<IPanelViewController> controllerList = new List<IPanelViewController>();

        public void AddController(IPanelSystemController controller)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveController(IPanelSystemController controller)
        {
            throw new System.NotImplementedException();
        }

        public void Push(IPanelViewController panelController)
        {
            panelController.ShowAsync(true).Wait();
            controllerList.Add(panelController);
        }

        public void Push(params IPanelViewController[] controllers)
        {
            throw new System.NotImplementedException();
        }

        public Task PushAsync(IPanelViewController controller)
        {
            throw new System.NotImplementedException();
        }

        public void Pop()
        {
            var controller = controllerList[controllerList.Count - 1];
            controllerList.RemoveAt(controllerList.Count-1);
            //return controller;
        }

        public Task PopAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Pop(int count)
        {
            throw new System.NotImplementedException();
        }

        public Task PopAsync(int count)
        {
            throw new System.NotImplementedException();
        }

        public void PopToIndex(int index)
        {
            throw new System.NotImplementedException();
        }

        public Task PopToIndexAsync(int index)
        {
            throw new System.NotImplementedException();
        }

        public void PopAndPush(int popCount, params IPanelViewController[] controllers)
        {
            throw new System.NotImplementedException();
        }

        public void PopAndPush(int popCount, IPanelViewController controller)
        {
            throw new System.NotImplementedException();
        }

        public Task PopAndPushAsync(int popCount, IPanelViewController controller)
        {
            throw new System.NotImplementedException();
        }

        public Task PopAndPushAsync(int popCount, params IPanelViewController[] controllers)
        {
            throw new System.NotImplementedException();
        }

        public Task PushAsync(params IPanelViewController[] controllers)
        {
            throw new System.NotImplementedException();
        }

        public Task ClearAndPushAsync(params IPanelViewController[] controllers)
        {
            throw new System.NotImplementedException();
        }

        public Task ClearAndPushAsync(IPanelViewController viewController)
        {
            throw new System.NotImplementedException();
        }

        public void ClearAndPush(params IPanelViewController[] controllers)
        {
            throw new System.NotImplementedException();
        }

        public void ClearAndPush(IPanelViewController viewController)
        {
            throw new System.NotImplementedException();
        }

        public Task ClearAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<IPanelViewController> GetEnumerator()
        {
            return controllerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => controllerList.Count;

        public IPanelViewController this[int index] => controllerList[index];
    }

    public class FakeViewContainer : IPanelViewContainer
    {
        public RectTransform ParentTransform { get; set;  }
    }

    public class FakeUIEventManager : IUIEventManager
    {
        public int LockCount { get; private set; }
        public int UnlockCount { get; private set; }

        public void Lock()
        {
            LockCount++;
        }

        public void Unlock()
        {
            UnlockCount++;
        }
    }

}
