using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace Gameframe.GUI.PanelSystem
{
    [RequireComponent(typeof(Animator),typeof(CanvasGroup))]
    public class QuickPanelAnimatorController : BasePanelAnimatorController, IPanelAnimator
    {
        public enum QuickAnimation
        {
            None,
            QuickFadeCanvasGroup,
            LongFadeCanvasGroup
        }
        
        [SerializeField]
        private QuickAnimation animationType = QuickAnimation.QuickFadeCanvasGroup;
        
        private int animationLayer = 0;
        
        private readonly int showAnimation = Animator.StringToHash("show");
        private readonly int hideAnimation = Animator.StringToHash("hide");
        
        public override async Task TransitionShowAsync()
        {
            await TransitionAsync(showAnimation, animationLayer);
        }

        public override async Task TransitionHideAsync()
        {
            await TransitionAsync(hideAnimation, animationLayer);
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