using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelViewController
    {
        PanelType PanelType { get; }
        bool IsViewLoaded { get; }
        Task LoadViewAsync();
        Task HideAsync();
        Task ShowAsync();
    }    
}
