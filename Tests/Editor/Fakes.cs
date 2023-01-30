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

        public Task HideAsync(bool immediate = false)
        {
            State = PanelViewControllerState.Disappeared;
            return Task.CompletedTask;
        }

        public Task ShowAsync(bool immediate = false)
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

        public void Pop()
        {
            var controller = controllerList[controllerList.Count - 1];
            controllerList.RemoveAt(controllerList.Count-1);
            //return controller;
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
