using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace Gameframe.GUI.PanelSystem
{
    [RequireComponent(typeof(Animator),typeof(CanvasGroup))]
    public class QuickPanelAnimatorController : MonoBehaviour, IPanelAnimator
    {
        public enum QuickAnimation
        {
            None,
            QuickFadeCanvasGroup,
            LongFadeCanvasGroup
        }
        
        [SerializeField]
        private QuickAnimation animationType = QuickAnimation.QuickFadeCanvasGroup;

        [SerializeField]
        private Animator animator = null;
        
        private int animationLayer = 0;
        
        private const string ShowAnimation = "show";
        private const string HideAnimation = "hide";
        
        public async Task TransitionShowAsync()
        {
            await TransitionAsync(ShowAnimation, animationLayer);
        }

        public async Task TransitionHideAsync()
        {
            await TransitionAsync(HideAnimation, animationLayer);
        }

        private async Task TransitionAsync(string stateName, int layer)
        {
            //Ensure Animator is Initialized
            while ((!animator.isInitialized || !animator.gameObject.activeInHierarchy) && Application.isPlaying)
            {
                await Task.Yield();
            }
            
            animator.Play(stateName);

            //Wait until we're in the target state
            while (!animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
            {
                await Task.Yield();
            }

            float normalizedTime = 0;
            while (normalizedTime < 1 && !Mathf.Approximately(normalizedTime,1f))
            {
                var info = animator.GetCurrentAnimatorStateInfo(0);
                if (normalizedTime <= info.normalizedTime)
                {
                    normalizedTime = info.normalizedTime;
                }
                else
                {
                    //If suddenly normalized time is less than 1 we've looped back around or something.
                    normalizedTime = 1f;
                }

                await Task.Yield();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (animator != null)
            {
                animator.runtimeAnimatorController = GetAnimatorController(animationType);
            }
        }
        private AnimatorController GetAnimatorController(QuickAnimation animationType)
        {
            var path = "Packages/com.gameframe.gui/Animators/";
            switch (animationType)
            {
                case QuickAnimation.QuickFadeCanvasGroup:
                    path += "quickfadecontroller.controller";
                    break;
                case QuickAnimation.LongFadeCanvasGroup:
                    path += "longfadecontroller.controller";
                    break;
                case QuickAnimation.None:
                    return null;
                default:
                    return null;
            }
            
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            
            if (controller == null)
            {
                Debug.Log($"Failed to load asset at path: {path}");    
            }

            return controller;
        }
#endif

    }
}