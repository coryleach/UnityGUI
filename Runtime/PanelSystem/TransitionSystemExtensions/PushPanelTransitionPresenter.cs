using System.Threading.Tasks;
using Gameframe.GUI.TransitionSystem;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// Pushes a panel controller onto the stack
    /// Use with SceneTransitionSystem
    /// </summary>
    public class PanelStackPushTransition : ITransitionPresenter
    {
        private readonly PanelType panelType;
        private readonly PanelViewControllerProvider provider;
        private readonly PanelStackSystem stack;

        public PanelStackPushTransition(PanelType panelType, PanelStackSystem stack, PanelViewControllerProvider provider)
        {
            this.panelType = panelType;
            this.provider = provider;
            this.stack = stack;
        }
    
        public Task StartTransitionAsync()
        {
            //Nothing to do yet. Push panel at the end of the transition.
            //Panel may want to use resources loaded in the new scene
            return Task.CompletedTask;
        }

        public void TransitionProgress(float progress)
        {
            //Nothing to do here.
            //Currently not supporting visualizing any progress for this transition
        }

        public async Task FinishTransitionAsync()
        {
            //Push the panel type provided and await the transition to complete before allowing transition to finish
            //We do this to make sure the panel has finished showing before finishing transition
            //This allows us to be sure a panel is showing before we drop a curtain
            var controller = provider.Get(panelType);
            await stack.PushAsync(controller); 
        }
    }   
}

