using NUnit.Framework;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public class AnimatedPanelViewTests
    {

        [Test]
        public void CanGetQuickAnimators(
            [Values(QuickPanelAnimatorController.QuickAnimation.LongFadeCanvasGroup, QuickPanelAnimatorController.QuickAnimation.QuickFadeCanvasGroup)]
            QuickPanelAnimatorController.QuickAnimation animationType)
        {
            var animator = QuickPanelAnimatorController.GetAnimatorController(animationType);
            Assert.IsTrue(animator != null);
        }
    
    }
}
