using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class AnimatedPanelView : PanelViewBase
    {
        private List<IPanelAnimator> animatorList = null;

        private IEnumerable<IPanelAnimator> GetAnimators()
        {
            if (animatorList == null)
            {
                animatorList = new List<IPanelAnimator>();
                GetComponents(animatorList);
            }
            return animatorList;
        }
    
        public override async Task ShowAsync(CancellationToken cancellationToken)
        {
            gameObject.SetActive(true);
            var animators = GetAnimators();
            var tasks = animators.Select(x => x.TransitionShowAsync());
            await Task.WhenAll(tasks);
        }

        public override async Task HideAsync(CancellationToken cancellationToken)
        {
            var animators = GetAnimators();
            var tasks = animators.Select(x => x.TransitionHideAsync());
            await Task.WhenAll(tasks);
            gameObject.SetActive(false);
        }

        public override void ShowImmediate()
        {
            gameObject.SetActive(true);   
        }

        public override void HideImmediate()
        {
            gameObject.SetActive(false);   
        }
    }   
}

