using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelViewContainer
    {
        RectTransform ParentTransform { get; }
    }
    
    public interface IPanelViewController
    {
        PanelType PanelType { get; }
        PanelViewBase View { get; }
        IPanelViewContainer ParentViewContainer { get; }
        bool IsViewLoaded { get; }
        Task LoadViewAsync();
        Task HideAsync();
        Task ShowAsync();
        void SetParentViewContainer(IPanelViewContainer parent);
    }
}
