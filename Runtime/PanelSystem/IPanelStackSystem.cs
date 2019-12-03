using System.Collections.Generic;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelStackSystem : IEnumerable<IPanelViewController>
    {
        int Count { get; }
        IPanelViewController this[int index] { get; }
    }    
}
