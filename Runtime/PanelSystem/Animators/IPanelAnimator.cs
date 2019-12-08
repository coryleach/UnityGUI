using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelAnimator
    {
        Task TransitionShowAsync();
        Task TransitionHideAsync();
    }
}