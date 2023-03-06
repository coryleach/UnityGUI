using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// PanelStackSystem maintains a stack of panel options
    /// Use together with a PanelStackController to create UI system with a history and a back button
    /// </summary>
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelStackSystem")]
    public class ScriptablePanelStackSystem : ScriptableObject, IPanelStackSystem
    {

        private PanelStackSystem _system = new PanelStackSystem();

        /// <summary>
        /// Clear stack and controllers list OnEnable
        /// Sometimes in editor these lists don't clear properly between play sessions
        /// </summary>
        private void OnEnable()
        {
            _system = new PanelStackSystem();
        }

        /// <summary>
        /// Number of panels in the stack
        /// </summary>
        public int Count => _system.Count;

        /// <summary>
        /// PanelViewController indexer
        /// </summary>
        /// <param name="index">index position in the stack of the controller to be returned</param>
        public IPanelViewController this[int index] => _system[index];

        /// <summary>
        /// PanelViewController on top of the stack
        /// </summary>
        public IPanelViewController CurrentTopPanel => _system.CurrentTopPanel;

        /// <summary>
        /// Add a panel stack controller to internal list of event subscribers
        /// </summary>
        /// <param name="controller">Controller to be added</param>
        public void AddController(IPanelSystemController controller) => _system.AddController(controller);

        /// <summary>
        /// Remove a panel stack Controller from list of stack event subscribers
        /// </summary>
        /// <param name="controller">Controller to be removed</param>
        public void RemoveController(IPanelSystemController controller)
        {
            _system.RemoveController(controller);
        }

        /// <summary>
        /// Push panel options onto top of panel stack
        /// </summary>
        /// <param name="controller"></param>
        public void Push(IPanelViewController controller)
        {
            _system.Push(controller);
        }

        /// <summary>
        /// Push panel options onto top of panel stack
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>Task that completes when panel is done being pushed</returns>
        public async Task PushAsync(IPanelViewController controller)
        {
            await _system.PushAsync(controller);
        }

        /// <summary>
        /// Pop the top panel off the stack
        /// </summary>
        public void Pop() => _system.Pop();

        /// <summary>
        /// Pop the top of the stack
        /// </summary>
        /// <returns>an awaitable task that completes when the transition between panels is complete</returns>
        public async Task PopAsync()
        {
            await _system.PopAsync();
        }

        /// <summary>
        /// Pop count number of panels from the top of the stack
        /// </summary>
        /// <param name="count">Number of panels to pop</param>
        /// <returns>Awaitable task that completes when the transition is done</returns>
        public async Task PopAsync(int count)
        {
            await _system.PopAsync(count);
        }

        /// <summary>
        /// Pop count number of panels from the top of the stack
        /// </summary>
        /// <param name="count">Number of panels to pop</param>
        public void Pop(int count) => _system.Pop(count);

        /// <summary>
        /// Pop stack to a specific index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Awaitable task that completes when the transition is done</returns>
        public async Task PopToIndexAsync(int index)
        {
            await _system.PopToIndexAsync(index);
        }

        /// <summary>
        /// Pop stack to a specific index
        /// </summary>
        /// <param name="index"></param>
        public void PopToIndex(int index) => _system.PopToIndex(index);

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controllers">list of controllers to push</param>
        public async Task PopAndPushAsync(int popCount, params IPanelViewController[] controllers)
        {
            await _system.PopAndPushAsync(popCount, controllers);
        }

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controllers">list of controllers to push</param>
        public void PopAndPush(int popCount, params IPanelViewController[] controllers) => _system.PopAndPush(popCount, controllers);

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controller">controller to push</param>
        public async Task PopAndPushAsync(int popCount, IPanelViewController controller)
        {
            await _system.PopAndPushAsync(popCount, controller);
        }

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controller">controller to push</param>
        public void PopAndPush(int popCount, IPanelViewController controller)
        {
            _system.PopAndPush(popCount, controller);
        }

        /// <summary>
        /// Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        /// <returns>Awaitable task that completes when the transition is complete</returns>
        public async Task PushAsync(params IPanelViewController[] controllers)
        {
            await _system.PushAsync(controllers);
        }

        /// <summary>
        /// Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        public void Push(params IPanelViewController[] controllers) => _system.Push(controllers);

        /// <summary>
        /// Clear the stack and Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        /// <returns>Awaitable task that completes when the transition is complete</returns>
        public async Task ClearAndPushAsync(params IPanelViewController[] controllers)
        {
            await _system.ClearAndPushAsync(controllers);
        }

        /// <summary>
        /// Clear the stack and Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        public void ClearAndPush(params IPanelViewController[] controllers) => _system.ClearAndPush(controllers);

        /// <summary>
        /// Clear all panels from the stack
        /// </summary>
        /// <returns>Awaitable task that completes when the panel transitions complete</returns>
        public async Task ClearAsync()
        {
            await _system.ClearAsync();
        }

        /// <summary>
        /// Clear all panels from the stack
        /// </summary>
        public void Clear() => _system.Clear();

        /// <summary>
        /// Clear all panels from the stack and then push a panel on top
        /// </summary>
        /// <returns>Awaitable task that completes when the panel transitions complete</returns>
        public async Task ClearAndPushAsync(IPanelViewController viewController)
        {
            await _system.ClearAndPushAsync(viewController);
        }

        /// <summary>
        /// Clear all panels from the stack and then push a panel on top
        /// </summary>
        public void ClearAndPush(IPanelViewController viewController) => _system.ClearAndPush(viewController);

        #region IEnumerable<PushPanelOptions>

        public IEnumerator<IPanelViewController> GetEnumerator()
        {
            return _system.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
