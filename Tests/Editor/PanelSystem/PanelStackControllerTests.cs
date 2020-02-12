using System.Collections;
using Gameframe.GUI.Camera.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public class PanelStackControllerTests
    {
        private static PanelStackController CreateStackController(IPanelStackSystem stackSystem = null, IPanelViewContainer viewContainer = null, IUIEventManager eventManager = null)
        {
            if (stackSystem == null)
            {
                stackSystem = new FakePanelStackSystem();
            }

            if (viewContainer == null)
            {
                viewContainer = new FakeViewContainer();
            }

            if (eventManager == null)
            {
                eventManager = new FakeUIEventManager();
            }
            
            return new PanelStackController(stackSystem,viewContainer,eventManager);
        }
        
        [Test]
        public void CanCreate()
        {
            var stackController = CreateStackController();
            Assert.IsTrue(stackController != null);
        }
        
        [Test]
        public void CanTransition()
        {
            var stackController = CreateStackController();
            stackController.TransitionAsync();
        }
        
        [UnityTest]
        public IEnumerator LocksAndUnlocksUIEventManager()
        {
            var stackSystem = new FakePanelStackSystem();
            var eventManager = new FakeUIEventManager();
            var stackController = CreateStackController(stackSystem, eventManager:eventManager);

            var panelController = new FakePanelViewController
            {
                PanelType = ScriptableObject.CreateInstance<PanelType>()
            };

            stackSystem.Push(panelController);
            Assert.IsTrue(stackSystem.Count == 1);
            
            yield return stackController.TransitionAsync().AsCoroutine();
            
            Assert.IsTrue(eventManager.LockCount == 1);
            Assert.IsTrue(eventManager.UnlockCount == 1);
        }
        
    }
}
