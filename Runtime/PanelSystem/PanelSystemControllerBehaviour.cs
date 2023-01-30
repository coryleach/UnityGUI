using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class PanelSystemControllerBehaviour : MonoBehaviour, IPanelSystemController, IPanelViewContainer
    {
        public virtual RectTransform ParentTransform => (RectTransform) transform;

        protected abstract IPanelSystem PanelSystem { get; }

        protected abstract IPanelSystemController BaseController { get; }

        protected virtual void OnEnable()
        {
            PanelSystem.AddController(this);
        }

        protected virtual void OnDisable()
        {
            PanelSystem.RemoveController(this);
        }

        public virtual  async Task TransitionAsync()
        {
            await BaseController.TransitionAsync();
        }
    }
}
