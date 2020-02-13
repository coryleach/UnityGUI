using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelAnimatorController : BasePanelAnimatorController
    {
        public int animationLayer = 0;
        public string showAnimation = "show";
        public string hideAnimation = "hide";
    
        public override async Task TransitionShowAsync()
        {
            await TransitionAsync(Animator.StringToHash(showAnimation), animationLayer);
        }

        public override async Task TransitionHideAsync()
        {
            await TransitionAsync(Animator.StringToHash(hideAnimation), animationLayer);
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

