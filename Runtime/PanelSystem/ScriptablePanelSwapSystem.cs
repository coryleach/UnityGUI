using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.PanelSystem
{
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelSwapSystem")]
    public class ScriptablePanelSwapSystem : ScriptableObject, IPanelSwapSystem
    {
        private PanelSwapSystem system = new PanelSwapSystem();

        public UnityEvent OnSwap => system.OnSwap;

        private void OnEnable()
        {
            //Clearing listeners because ScriptableObject will hold onto old subscribers
            //when running in editor
            system.OnSwap.RemoveAllListeners();
        }

        public IPanelViewController CurrentViewController => system.CurrentViewController;

        public void AddController(IPanelSwapController controller) => system.AddController(controller);

        public void RemoveController(IPanelSwapController controller) => system.RemoveController(controller);

        public void Show(IPanelViewController controller) => system.Show(controller);

        public async Task ShowAsync(IPanelViewController panelViewController)
        {
            await system.ShowAsync(panelViewController);
        }
    }
}
