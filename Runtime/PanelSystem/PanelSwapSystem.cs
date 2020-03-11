using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelSwapSystem")]
    public class PanelSwapSystem : ScriptableObject, IPanelSwapSystem
    {
        private readonly List<IPanelSwapController> controllers = new List<IPanelSwapController>();
        
        private IPanelViewController currentViewController = null;
        public IPanelViewController CurrentViewController => currentViewController;

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
        }
    }
}

