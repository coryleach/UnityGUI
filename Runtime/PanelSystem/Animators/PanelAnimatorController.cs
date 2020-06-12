using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelAnimatorController : BasePanelAnimatorController
    {
        [SerializeField]
        private int animationLayer;
        [SerializeField]
        private string showAnimation = "show";
        [SerializeField]
        private string hideAnimation = "hide";
    
        public override async Task TransitionShowAsync()
        {
            await TransitionAsync(Animator.StringToHash(showAnimation), animationLayer);
        }

        public override async Task TransitionHideAsync()
        {
            await TransitionAsync(Animator.StringToHash(hideAnimation), animationLayer);
        }
        
        public void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }
    }
}

