namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelSystem
    {
        void AddController(IPanelSystemController controller);
        void RemoveController(IPanelSystemController controller);
    }
}