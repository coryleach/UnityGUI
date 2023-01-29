using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.PanelSystem
{
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelSwapSystem")]
    public class ScriptablePanelSwapSystem : ScriptableObject, IPanelSwapSystem
    {
        private readonly PanelSwapSystem _system = new PanelSwapSystem();

        public UnityEvent OnSwap => _system.OnSwap;

        private void OnEnable()
        {
            //Clearing listeners because ScriptableObject will hold onto old subscribers
            //when running in editor
            _system.OnSwap.RemoveAllListeners();
        }

        public IPanelViewController CurrentViewController => _system.CurrentViewController;

        public void AddController(IPanelSwapController controller) => _system.AddController(controller);

        public void RemoveController(IPanelSwapController controller) => _system.RemoveController(controller);

        public void Show(IPanelViewController controller) => _system.Show(controller);

        public async Task ShowAsync(IPanelViewController panelViewController)
        {
            await _system.ShowAsync(panelViewController);
        }
    }
}
