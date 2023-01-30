using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gameframe.GUI.PanelSystem
{
    /// <summary>
    /// PanelStackSystem maintains a stack of panel options
    /// Use together with a PanelStackController to create UI system with a history and a back button
    /// </summary>
    public class PanelStackSystem : IPanelStackSystem
    {
        private readonly List<IPanelViewController> stack = new List<IPanelViewController>();
        private readonly List<IPanelSystemController> systemControllers = new List<IPanelSystemController>();

        /// <summary>
        /// Number of panels in the stack
        /// </summary>
        public int Count => stack.Count;

        /// <summary>
        /// PanelViewController indexer
        /// </summary>
        /// <param name="index">index position in the stack of the controller to be returned</param>
        public IPanelViewController this[int index] => stack[index];

        /// <summary>
        /// PanelViewController on top of the stack
        /// </summary>
        public IPanelViewController CurrentTopPanel => stack.Count == 0 ? null : stack[stack.Count - 1];

        /// <summary>
        /// Add a panel stack controller to internal list of event subscribers
        /// </summary>
        /// <param name="controller">Controller to be added</param>
        public void AddController(IPanelSystemController controller)
        {
            systemControllers.Add(controller);
        }

        /// <summary>
        /// Remove a panel stack Controller from list of stack event subscribers
        /// </summary>
        /// <param name="controller">Controller to be removed</param>
        public void RemoveController(IPanelSystemController controller)
        {
            systemControllers.Remove(controller);
        }

        /// <summary>
        /// Push panel options onto top of panel stack
        /// </summary>
        /// <param name="controller"></param>
        public async void Push(IPanelViewController controller)
        {
            await PushAsync(controller).ConfigureAwait(false);
        }

        /// <summary>
        /// Push panel options onto top of panel stack
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Task that completes when panel is done being pushed</returns>
        public async Task PushAsync(IPanelViewController controller)
        {
            stack.Add(controller);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Pop the top panel off the stack
        /// </summary>
        public async void Pop()
        {
            await PopAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Pop the top of the stack
        /// </summary>
        /// <returns>an awaitable task that completes when the transition between panels is complete</returns>
        public async Task PopAsync()
        {
            if (stack.Count == 0)
            {
                return;
            }

            stack.RemoveAt(stack.Count - 1);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Pop count number of panels from the top of the stack
        /// </summary>
        /// <param name="count">Number of panels to pop</param>
        /// <returns>Awaitable task that completes when the transition is done</returns>
        public async Task PopAsync(int count)
        {
            stack.RemoveRange(stack.Count-count,count);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Pop count number of panels from the top of the stack
        /// </summary>
        /// <param name="count">Number of panels to pop</param>
        public async void Pop(int count)
        {
            await PopAsync(count).ConfigureAwait(false);
        }

        /// <summary>
        /// Pop stack to a specific index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Awaitable task that completes when the transition is done</returns>
        public async Task PopToIndexAsync(int index)
        {
            if ((index+1) < stack.Count)
            {
                stack.RemoveRange(index+1, stack.Count - (index+1));
            }
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Pop stack to a specific index
        /// </summary>
        /// <param name="index"></param>
        public async void PopToIndex(int index)
        {
            await PopToIndexAsync(index).ConfigureAwait(false);
        }

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controllers">list of controllers to push</param>
        public async Task PopAndPushAsync(int popCount, params IPanelViewController[] controllers)
        {
            if (popCount > stack.Count)
            {
                popCount = stack.Count;
            }
            stack.RemoveRange(stack.Count-popCount,popCount);
            stack.AddRange(controllers);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controllers">list of controllers to push</param>
        public async void PopAndPush(int popCount, params IPanelViewController[] controllers)
        {
            await PopAndPushAsync(popCount, controllers).ConfigureAwait(false);
        }

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controller">controller to push</param>
        public async Task PopAndPushAsync(int popCount, IPanelViewController controller)
        {
            if (popCount > stack.Count)
            {
                popCount = stack.Count;
            }
            stack.RemoveRange(stack.Count-popCount,popCount);
            stack.Add(controller);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// This method allows popping and pushing as one action with no transition required between.
        /// </summary>
        /// <param name="popCount">number of panels to pop</param>
        /// <param name="controller">controller to push</param>
        public async void PopAndPush(int popCount, IPanelViewController controller)
        {
            await PopAndPushAsync(popCount, controller).ConfigureAwait(false);
        }

        /// <summary>
        /// Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        /// <returns>Awaitable task that completes when the transition is complete</returns>
        public async Task PushAsync(params IPanelViewController[] controllers)
        {
            stack.AddRange(controllers);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        public async void Push(params IPanelViewController[] controllers)
        {
            await PushAsync(controllers).ConfigureAwait(false);
        }

        /// <summary>
        /// Clear the stack and Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        /// <returns>Awaitable task that completes when the transition is complete</returns>
        public async Task ClearAndPushAsync(params IPanelViewController[] controllers)
        {
            stack.Clear();
            stack.AddRange(controllers);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Clear the stack and Push a set of panels async
        /// </summary>
        /// <param name="controllers">array of panel view controllers</param>
        public async void ClearAndPush(params IPanelViewController[] controllers)
        {
            await ClearAndPushAsync(controllers).ConfigureAwait(false);
        }

        /// <summary>
        /// Clear all panels from the stack
        /// </summary>
        /// <returns>Awaitable task that completes when the panel transitions complete</returns>
        public async Task ClearAsync()
        {
            stack.Clear();
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Clear all panels from the stack
        /// </summary>
        public async void Clear()
        {
            await ClearAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Clear all panels from the stack and then push a panel on top
        /// </summary>
        /// <returns>Awaitable task that completes when the panel transitions complete</returns>
        public async Task ClearAndPushAsync(IPanelViewController viewController)
        {
            stack.Clear();
            stack.Add(viewController);
            await TransitionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Clear all panels from the stack and then push a panel on top
        /// </summary>
        public async void ClearAndPush(IPanelViewController viewController)
        {
            await ClearAndPushAsync(viewController).ConfigureAwait(false);
        }

        private async Task TransitionAsync()
        {
            if (systemControllers.Count == 1)
            {
                await systemControllers[0].TransitionAsync();
            }
            else
            {
                var tasks = new Task[systemControllers.Count];

                for (var i = 0; i < systemControllers.Count; i++)
                {
                    tasks[i] = systemControllers[i].TransitionAsync();
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        #region IEnumerable<PushPanelOptions>

        public IEnumerator<IPanelViewController> GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
