using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelStackSystem : IEnumerable<IPanelViewController>, IPanelSystem
    {
        int Count { get; }
        IPanelViewController this[int index] { get; }
        void Push(IPanelViewController controller);
        void Push(params IPanelViewController[] controllers);
        Task PushAsync(IPanelViewController controller);
        void Pop();
        Task PopAsync();
        void Pop(int count);
        Task PopAsync(int count);
        void PopToIndex(int index);
        Task PopToIndexAsync(int index);
        void PopAndPush(int popCount, params IPanelViewController[] controllers);
        void PopAndPush(int popCount, IPanelViewController controller);
        Task PopAndPushAsync(int popCount, IPanelViewController controller);
        Task PopAndPushAsync(int popCount, params IPanelViewController[] controllers);
        Task PushAsync(params IPanelViewController[] controllers);
        Task ClearAndPushAsync(params IPanelViewController[] controllers);
        Task ClearAndPushAsync(IPanelViewController viewController);
        void ClearAndPush(params IPanelViewController[] controllers);
        void ClearAndPush(IPanelViewController viewController);
        Task ClearAsync();
        void Clear();
    }
}
