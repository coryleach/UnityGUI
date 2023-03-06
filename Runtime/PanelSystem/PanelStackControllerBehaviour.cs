using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// PanelStackControllerBehaviour is a MonoBehaviour wrapper around a PanelStackController instance
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class PanelStackControllerBehaviour : PanelSystemControllerBehaviour, IPanelStackController
    {
        [SerializeField]
        private UIEventManager eventManager;

        [SerializeField]
        private ScriptablePanelStackSystem panelStackSystem;

        public ScriptablePanelStackSystem System => panelStackSystem;

        [SerializeField, Tooltip("If true the panel stack will be cleared when this object is destroyed. This may be desired when reloading the game for example.")]
        protected bool clearSystemStackOnDestroy = true;

        private PanelStackController _baseController;
        protected override IPanelSystemController BaseController =>
            _baseController ??= new PanelStackController(panelStackSystem, this, eventManager);

        protected override IPanelSystem PanelSystem => panelStackSystem;

        protected virtual void OnDestroy()
        {
            if (clearSystemStackOnDestroy)
            {
                panelStackSystem.Clear();
            }
        }
    }

}
