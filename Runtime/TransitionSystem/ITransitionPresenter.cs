using System.Threading.Tasks;

namespace Gameframe.GUI.TransitionSystem
{
    public interface ITransitionPresenter
    {
        Task StartTransitionAsync();
        void TransitionProgress(float progress);
        Task FinishTransitionAsync();
    }
}