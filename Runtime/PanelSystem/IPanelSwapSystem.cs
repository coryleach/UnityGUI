using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelSwapSystem
    {
        IPanelViewController CurrentViewController { get; }
        void AddController(IPanelSwapController controller);
        void RemoveController(IPanelSwapController controller);
        void Show(IPanelViewController panelViewController);
        Task ShowAsync(IPanelViewController panelViewController);
    }
}


