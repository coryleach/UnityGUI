using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class AnimatedPanelView : PanelViewBase
    {
        private List<IPanelAnimator> animatorList;

        private IEnumerable<IPanelAnimator> GetAnimators()
        {
            if (animatorList != null)
            {
                return animatorList;
            }
            
            animatorList = new List<IPanelAnimator>();
            GetComponents(animatorList);
            return animatorList;
        }
    
        public override async Task ShowAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            
            gameObject.SetActive(true);
            var animators = GetAnimators();
            var tasks = animators.Select(x => x.TransitionShowAsync());
            await Task.WhenAll(tasks);
        }

        public override async Task HideAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested || !gameObject.activeSelf)
            {
                return;
            }
            
            var animators = GetAnimators();
            var tasks = animators.Select(x => x.TransitionHideAsync());
            
            await Task.WhenAll(tasks);

            if (cancellationToken.IsCancellationRequested || !Application.isPlaying)
            {
                return;
            }
            
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

