using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public class FakePanelStackSystem : IPanelStackSystem
    {
        public IEnumerator<IPanelViewController> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; }

        public IPanelViewController this[int index] => throw new System.NotImplementedException();
    }

    public class FakeViewContainer : IPanelViewContainer
    {
        public RectTransform ParentTransform { get; set;  }
    }
    
    public class PanelStackControllerTests
    {
        private static PanelStackController CreateStackController()
        {
            var stackSystem = new FakePanelStackSystem();
            var container = new FakeViewContainer();
            return new PanelStackController(stackSystem,container);
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
    }
}
