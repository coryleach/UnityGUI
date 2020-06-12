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
        private readonly PanelStackSystem _stack;

        public PanelStackPopTransition(PanelStackSystem stack)
        {
            _stack = stack;
        }
    
        public Task StartTransitionAsync()
        {
            return Task.CompletedTask;
        }

        public void TransitionProgress(float progress)
        {
            //Progress display not currently supported for this transition presenter
        }

        public async Task FinishTransitionAsync()
        {
            await _stack.PopAsync(); 
        }
    }    
}
