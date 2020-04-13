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
        private readonly PanelStackSystem stack;

        public PanelStackPopTransition(PanelStackSystem stack)
        {
            this.stack = stack;
        }
    
        public Task StartTransitionAsync()
        {
            return Task.CompletedTask;
        }

        public void TransitionProgress(float progress)
        {
        }

        public async Task FinishTransitionAsync()
        {
            await stack.PopAsync(); 
        }
    }    
}
