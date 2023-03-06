using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelViewController
    {
        PanelViewControllerState State { get; }
        PanelType PanelType { get; }
        PanelViewBase View { get; }
        IPanelViewContainer ParentViewContainer { get; }
        bool IsViewLoaded { get; }
        Task LoadViewAsync();
        Task UnloadViewAsync();
        Task HideAsync(bool immediate = false, ITransitionEvent transitionEvent = null);
        Task ShowAsync(bool immediate = false, ITransitionEvent transitionEvent = null);
        void SetParentViewContainer(IPanelViewContainer parent);
    }
}
