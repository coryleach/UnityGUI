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
            return Task.CompletedTask;
        }

        public void TransitionProgress(float progress)
        {
        }

        public async Task FinishTransitionAsync()
        {
            var controller = provider.Get(panelType);
            await stack.PushAsync(controller); 
        }
    }   
}

