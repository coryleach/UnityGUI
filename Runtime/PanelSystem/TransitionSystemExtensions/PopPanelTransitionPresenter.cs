using System.Threading.Tasks;
using Gameframe.GUI.TransitionSystem;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// Pops a panel from a stack before finishing a Scene transition
    /// For use with SceneTransitionSystem class.
    /// </summary>
    public class PanelStackPopTransition : ITransitionPresenter
    {
        private readonly ScriptablePanelStackSystem _stack;

        public PanelStackPopTransition(ScriptablePanelStackSystem stack)
        {
            _stack = stack;
        }

        public Task StartTransitionAsync()
        {
            //Nothing to do on start transition.
            //We don't pop until the transition is ready to finish
            return Task.CompletedTask;
        }

        public Task PreTransitionAsync()
        {
            return Task.FromResult(true);
        }

        public void TransitionProgress(float progress)
        {
            //Progress display not currently supported for this transition presenter
        }

        public Task PostTransitionAsync()
        {
            return Task.FromResult(true);
        }

        public async Task FinishTransitionAsync()
        {
            //Transition is ready to finish
            //This will pop a panel off the stack and then allow the transition to complete
            //For example we might not want to have a scene transition curtain raise till a panel is popped
            await _stack.PopAsync();
        }
    }
}
