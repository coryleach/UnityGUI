using System.Collections.Generic;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelStackSystem : IEnumerable<IPanelViewController>, IPanelSystem
    {
        int Count { get; }
        IPanelViewController this[int index] { get; }
        void Push(IPanelViewController controller);
        void Pop();
    }
}
