namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelViewControllerProvider
    {
        T Get<T>(PanelType type) where T : IPanelViewController;
        IPanelViewController Get(PanelType type);
    }
}

