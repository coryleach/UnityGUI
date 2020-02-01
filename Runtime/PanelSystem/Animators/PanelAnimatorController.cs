using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelAnimatorController : MonoBehaviour, IPanelAnimator
    {
        public Animator animator = null;
        public int animationLayer = 0;
        public string showAnimation = "show";
        public string hideAnimation = "hide";
    
        public async Task TransitionShowAsync()
        {
            await TransitionAsync(showAnimation, animationLayer);
        }

        public async Task TransitionHideAsync()
        {
            await TransitionAsync(hideAnimation, animationLayer);
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

        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
    }
}

