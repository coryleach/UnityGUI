using NUnit.Framework;
using UnityEngine;

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

        [Test]
        public void MathTest()
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            
            float x = 1;
            var val1 = n1 * (x -= 1.5f / d1) * x + 0.75f;

            x = 1;
            x -= 1.5f / d1;
            var val2 =  n1 * x * x + 0.75f;
            
            Assert.IsTrue(Mathf.Approximately(val1,val2));
        }
        
    }
}
