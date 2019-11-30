using System.Threading;
using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// Basic panel view with instant transitions
    /// </summary>
    public class PanelView : PanelViewBase
    {
        public override Task ShowAsync(CancellationToken cancellationToken)
        {
            gameObject.SetActive(true);
            return Task.CompletedTask;
        }

        public override Task HideAsync(CancellationToken cancellationToken)
        {
            gameObject.SetActive(false);
            return Task.CompletedTask;
        }
    }
}

