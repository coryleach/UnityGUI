using Gameframe.GUI.PanelSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
#if UNITY_EDITOR
using System.Collections;
using System.Threading;
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace Gameframe.GUI.Tests.Runtime
{
    public class AnimatedPanelViewTests
    {
        
        #if UNITY_EDITOR

        /// <summary>
        /// Loads an animator controller from assets for test
        /// </summary>
        /// <returns></returns>
        private AnimatorController GetAnimatorController()
        {
         return QuickPanelAnimatorController.GetAnimatorController(QuickPanelAnimatorController.QuickAnimation
             .QuickFadeCanvasGroup);   
        }
        
        [Test]
        public void CanGetAnimatorController()
        {
            Assert.IsTrue(GetAnimatorController() != null);
        }
        
        [UnityTest, Timeout(10000)]
        public IEnumerator CanAnimatePanel()
        {
            var gameObject = new GameObject();

            //Used by the animation
            var canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
            //Setup Animator
            var animator = gameObject.AddComponent<Animator>();
            var animatorController = GetAnimatorController();
            animator.runtimeAnimatorController = animatorController;

            var panelView = gameObject.AddComponent<AnimatedPanelView>();
            gameObject.AddComponent<PanelAnimatorController>();
            
            Assert.IsTrue(animatorController != null);

            int count = 3;
            for (int i = 0; i < count; i++)
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource())
                {
                    var task = panelView.ShowAsync(tokenSource.Token);
                    while (!task.IsCompleted)
                    {
                        yield return null;
                    }
                }

                Assert.IsTrue(Mathf.Approximately(canvasGroup.alpha, 1));
                Assert.IsTrue(panelView.gameObject.activeSelf);

                using (CancellationTokenSource tokenSource = new CancellationTokenSource())
                {
                    var task = panelView.HideAsync(tokenSource.Token);
                    while (!task.IsCompleted)
                    {
                        yield return null;
                    }
                }

                Assert.IsTrue(Mathf.Approximately(canvasGroup.alpha, 0),
                    $"Expected Alpha 0. Alpha = {canvasGroup.alpha}");
                Assert.IsFalse(panelView.gameObject.activeSelf);
            }
        }
        
        #endif
    }
}

