using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelStackController
    {
        /// <summary>
        /// Transitions views to reflect state of the panel stack system
        /// </summary>
        /// <returns>Task that completes when transition is finished</returns>
        Task TransitionAsync();
    }
}
