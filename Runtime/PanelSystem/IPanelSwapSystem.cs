using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelSwapSystem : IPanelSystem
    {
        IPanelViewController CurrentViewController { get; }
        void Show(IPanelViewController panelViewController);
        Task ShowAsync(IPanelViewController panelViewController);
    }
}
