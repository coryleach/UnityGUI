using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelSwapSystem : IPanelSwapSystem
    {
        private readonly List<IPanelSystemController> _controllers = new List<IPanelSystemController>();

        private IPanelViewController _currentViewController;
        public IPanelViewController CurrentViewController => _currentViewController;

        private readonly UnityEvent onSwap = new UnityEvent();
        public UnityEvent OnSwap => onSwap;

        public void AddController(IPanelSystemController controller)
        {
            _controllers.Add(controller);
        }

        public void RemoveController(IPanelSystemController controller)
        {
            _controllers.Add(controller);
        }

        public async void Show(IPanelViewController controller)
        {
            await ShowAsync(controller).ConfigureAwait(false);
        }

        public async Task ShowAsync(IPanelViewController panelViewController)
        {
            _currentViewController = panelViewController;
            foreach (var swapController in _controllers)
            {
                await swapController.TransitionAsync();
            }
            onSwap.Invoke();
        }
    }
}
