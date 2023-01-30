using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelViewContainer
    {
        RectTransform ParentTransform { get; }
    }
}