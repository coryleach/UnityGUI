using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelSystemController
    {
        /// <summary>
        /// Transitions views to reflect state of the panel system
        /// </summary>
        /// <returns>Task that completes when transition is finished</returns>
        Task TransitionAsync();
    }

    public interface ITransitionEvent
    {

    }
}
