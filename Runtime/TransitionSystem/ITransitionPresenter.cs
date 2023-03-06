using System.Threading.Tasks;

namespace Gameframe.GUI.TransitionSystem
{
    public interface ITransitionPresenter
    {
        /// <summary>
        /// Called when a transition starts.
        /// For example: raising the transition curtain
        /// </summary>
        /// <returns>awaitable task</returns>
        Task StartTransitionAsync();

        /// <summary>
        /// Called before transition is begun.
        /// Use this to do any logic required before transition.
        /// For example: save the game or player position before switching scenes
        /// </summary>
        /// <returns>awaitable task</returns>
        Task PreTransitionAsync();

        /// <summary>
        /// Called periodically with a value representing the progress of the transition
        /// </summary>
        /// <param name="progress">value 0 to 1 representing the progress of the transition with 1 = 100%</param>
        void TransitionProgress(float progress);

        /// <summary>
        /// Runs after transition progress as reached 100%
        /// Use this method to run post transition logic
        /// For example: spawning the player and attaching the camera before raising the transition curtain
        /// </summary>
        /// <returns>awaitable task</returns>
        Task PostTransitionAsync();

        /// <summary>
        /// Runs as the final step of the transition after all presenters have completed the all the previous steps
        /// </summary>
        /// <returns>awaitable task</returns>
        Task FinishTransitionAsync();
    }
}
