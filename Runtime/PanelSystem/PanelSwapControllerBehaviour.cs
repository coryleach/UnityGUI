using Gameframe.GUI.Camera.UI;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public class PanelSwapControllerBehaviour : PanelSystemControllerBehaviour, IPanelSwapController
    {
        [SerializeField]
        private UIEventManager eventManager;

        [SerializeField]
        private ScriptablePanelSwapSystem panelSwapSystem;

        public ScriptablePanelSwapSystem System => panelSwapSystem;

        protected override IPanelSystem PanelSystem => panelSwapSystem;

        private PanelSwapController _baseController;
        protected override IPanelSystemController BaseController =>
            _baseController ??= new PanelSwapController(panelSwapSystem, this, eventManager);

    }
}
