using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelSwapControllerBehaviour : MonoBehaviour, IPanelSwapController, IPanelViewContainer
    {
        [SerializeField] 
        private UIEventManager eventManager;
        
        [SerializeField] 
        private PanelSwapSystem panelSwapSystem;

        public PanelSwapSystem System => panelSwapSystem;
        
        private PanelSwapController baseController;

        public RectTransform ParentTransform => (RectTransform) transform;
        
        private PanelSwapController BaseController =>
            baseController ??
            (baseController = new PanelSwapController(panelSwapSystem, this, eventManager));

        protected virtual void OnEnable()
        {
            panelSwapSystem.AddController(this);
        }
        
        protected virtual void OnDisable()
        {
            panelSwapSystem.RemoveController(this);
        }
        
        public virtual async Task TransitionAsync()
        {
            await BaseController.TransitionAsync();
        }
        
    }
    
}

