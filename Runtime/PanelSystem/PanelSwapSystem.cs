using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.PanelSystem
{
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelSwapSystem")]
    public class PanelSwapSystem : ScriptableObject, IPanelSwapSystem
    {
        private readonly List<IPanelSwapController> _controllers = new List<IPanelSwapController>();
        
        private IPanelViewController _currentViewController;
        public IPanelViewController CurrentViewController => _currentViewController;

        private readonly UnityEvent onSwap = new UnityEvent();
        public UnityEvent OnSwap => onSwap;

        private void OnEnable()
        {
            //Clearing listeners because ScriptableObject will hold onto old subscribers
            //when running in editor
            onSwap.RemoveAllListeners();
        }

        public void AddController(IPanelSwapController controller)
        {
            _controllers.Add(controller);
        }

        public void RemoveController(IPanelSwapController controller)
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

