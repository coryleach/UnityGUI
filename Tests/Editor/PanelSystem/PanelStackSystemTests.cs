using NUnit.Framework;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public class PanelStackSystemTests
    {
        
        private static ScriptablePanelStackSystem CreatePanelStackSystem()
        {
            return ScriptableObject.CreateInstance<ScriptablePanelStackSystem>();
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
