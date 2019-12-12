using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public class FakePanelViewController : IPanelViewController
    {
        public PanelType PanelType => null;

        public PanelViewBase View => null;

        public IPanelViewContainer ParentViewContainer => null;

        private bool loaded = false;

        public bool IsViewLoaded => loaded;
            
        public Task LoadViewAsync()
        {
            loaded = true;
            return Task.CompletedTask;
        }

        public Task HideAsync()
        {
            return Task.CompletedTask;
        }

        public Task ShowAsync()
        {
            return Task.CompletedTask;
        }

        public void SetParentViewContainer(IPanelViewContainer parent)
        {
        }
    }

    public class FakePanelStackController : IPanelStackController
    {
        public bool transitioned = false;
        public Task TransitionAsync()
        {
            transitioned = true;
            return Task.CompletedTask;
        }
    }
    
    public class PanelStackSystemTests
    {
        
        private static PanelStackSystem CreatePanelStackSystem()
        {
            return ScriptableObject.CreateInstance<PanelStackSystem>();
        }

        [Test]
        public void CanCreate()
        {
            var stackSystem = CreatePanelStackSystem();
            Assert.IsTrue(stackSystem != null);
        }

        [Test]
        public void PanelStackControllerCanSubscribeAndUnsubscribeAndTransition()
        {
            var stackSystem = CreatePanelStackSystem();
            var stackController = new FakePanelStackController();
            var panelController = new FakePanelViewController();
            
            //Can Subscribe
            stackSystem.AddController(stackController);

            stackController.transitioned = false;
            stackSystem.Push(panelController);
            //Ensure we transitioned on push
            Assert.IsTrue(stackController.transitioned);

            stackController.transitioned = false;
            stackSystem.Pop();
            //Ensure we transitioned on pop
            Assert.IsTrue(stackController.transitioned);

            //Can Unsubscribe
            stackSystem.RemoveController(stackController);
                        
            stackController.transitioned = false;
            stackSystem.Push(panelController);
            Assert.IsTrue(!stackController.transitioned);

            stackController.transitioned = false;
            stackSystem.Pop();
            Assert.IsTrue(!stackController.transitioned);
        }
        
        [Test]
        public void PanelStackControllerCanPush()
        {
            var stackSystem = CreatePanelStackSystem();
            
            var panelController_0 = new FakePanelViewController();
            var panelController_1 = new FakePanelViewController();

            Assert.IsTrue(stackSystem.Count == 0);
            stackSystem.Push(panelController_0);
            Assert.IsTrue(stackSystem.Count == 1);
            Assert.IsTrue(stackSystem[0] == panelController_0);
            
            stackSystem.Push(panelController_1);
            Assert.IsTrue(stackSystem.Count == 2);
            Assert.IsTrue(stackSystem[0] == panelController_0);
            
            //Ensure the latest pushed panel is on the top
            Assert.IsTrue(stackSystem[1] == panelController_1);
        }

        [Test]
        public void PanelStackControllerCanPop()
        {
            var stackSystem = CreatePanelStackSystem();
            
            var panelController_0 = new FakePanelViewController();
            var panelController_1 = new FakePanelViewController();

            stackSystem.Push(panelController_0);
            stackSystem.Push(panelController_1);
            
            Assert.IsTrue(stackSystem.Count == 2);
            Assert.IsTrue(stackSystem[0] == panelController_0);
            Assert.IsTrue(stackSystem[1] == panelController_1);
            
            stackSystem.Pop();
            Assert.IsTrue(stackSystem.Count == 1);
            Assert.IsTrue(stackSystem[0] == panelController_0);
            
            stackSystem.Pop();
            Assert.IsTrue(stackSystem.Count == 0);

        }
        

    }
}
