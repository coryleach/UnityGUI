using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public abstract class PanelViewBase : MonoBehaviour
    {
        public abstract Task ShowAsync(CancellationToken cancellationToken);
        public abstract Task HideAsync(CancellationToken cancellationToken);
    }
}


