using System.Threading.Tasks;

namespace Gameframe.GUI.TransitionSystem
{
    public interface ITransitionTask
    {
        float Progress { get; }
        Task ExecuteAsync();
    }
}