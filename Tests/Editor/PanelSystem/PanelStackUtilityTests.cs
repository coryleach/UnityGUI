using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public class PanelStackUtilityTests
    {
        [Test]
        public void GetVisiblePanelViewControllers()
        {
            var fakeSystem = new FakePanelStackSystem();
            var showControllersList = new List<IPanelViewController>();

            var opaquePanelType = ScriptableObject.CreateInstance<PanelType>();
            opaquePanelType.visibility = PanelType.Visibility.Opaque;

            var transparentPanelType = ScriptableObject.CreateInstance<PanelType>();
            transparentPanelType.visibility = PanelType.Visibility.Transparent;

            PanelStackUtility.GetVisiblePanelViewControllers(fakeSystem,showControllersList);
            Assert.IsTrue(showControllersList.Count == 0);

            fakeSystem.Push(new FakePanelViewController { PanelType = opaquePanelType });
            PanelStackUtility.GetVisiblePanelViewControllers(fakeSystem,showControllersList);
            Assert.IsTrue(showControllersList.Count == 1);

            //Should have just one showing panel as long as we're showing opaque panels
            fakeSystem.Push(new FakePanelViewController { PanelType = opaquePanelType });
            PanelStackUtility.GetVisiblePanelViewControllers(fakeSystem,showControllersList);
            Assert.IsTrue(showControllersList.Count == 1,$"Got ShowControllersList Size: {showControllersList.Count}");
            
            //Pushing a transparent panel should show more controllers
            fakeSystem.Push(new FakePanelViewController { PanelType = transparentPanelType });
            PanelStackUtility.GetVisiblePanelViewControllers(fakeSystem,showControllersList);
            Assert.IsTrue(showControllersList.Count == 2,$"Got ShowControllersList Size: {showControllersList.Count}");
            
            //Pushing a transparent panel should show more controllers
            fakeSystem.Push(new FakePanelViewController { PanelType = transparentPanelType });
            PanelStackUtility.GetVisiblePanelViewControllers(fakeSystem,showControllersList);
            Assert.IsTrue(showControllersList.Count == 3,$"Got ShowControllersList Size: {showControllersList.Count}");

            //Pushing Opaque should now hide all lower panels
            fakeSystem.Push(new FakePanelViewController { PanelType = opaquePanelType });
            PanelStackUtility.GetVisiblePanelViewControllers(fakeSystem,showControllersList);
            Assert.IsTrue(showControllersList.Count == 1,$"Got ShowControllersList Size: {showControllersList.Count}");
        }
    }

}