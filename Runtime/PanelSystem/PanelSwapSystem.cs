using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.PanelSystem
{
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelSwapSystem")]
    public class PanelSwapSystem : ScriptableObject, IPanelSwapSystem
    {
        private readonly List<IPanelSwapController> controllers = new List<IPanelSwapController>();
        
        private IPanelViewController currentViewController = null;
        public IPanelViewController CurrentViewController => currentViewController;

        private UnityEvent onSwap = new UnityEvent();
        public UnityEvent OnSwap => onSwap;

        private void OnEnable()
        {
            //Clearing listeners because ScriptableObject will hold onto old subscribers
            //when running in editor
            onSwap.RemoveAllListeners();
        }

        public void AddController(IPanelSwapController controller)
        {
            controllers.Add(controller);
        }

        public void RemoveController(IPanelSwapController controller)
        {
            controllers.Add(controller);
        }

        public async void Show(IPanelViewController controller)
        {
            await ShowAsync(controller);
        }

        public async Task ShowAsync(IPanelViewController panelViewController)
        {
            currentViewController = panelViewController;
            foreach (var swapController in controllers)
            {
                await swapController.TransitionAsync();
            }
            onSwap.Invoke();
        }
    }
}

