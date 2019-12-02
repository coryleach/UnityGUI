using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// TODO: Should be able to make this obsolete.
    /// Should support pushing of panel view controllers directly to the navigation stack
    /// We should have a PanelSystem or PanelProvider that is a service which will instantiate and provide PanelViewController objects
    /// </summary>
    public class PushPanelOptions
    {
        public int? overrideSortLayer;
        public PanelType panelType;
    }
}
