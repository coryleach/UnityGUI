using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelStackSystem")]
    public class PanelStackSystem : ScriptableObject
    {
        private readonly List<PushPanelOptions> stack = new List<PushPanelOptions>();
        private readonly List<IPanelStackController> stackControllers = new List<IPanelStackController>();

        private void OnEnable()
        {
            stack.Clear();
            stackControllers.Clear();
        }

        /// <summary>
        /// Add a panel stack controller to internal list of event subscribers
        /// </summary>
        /// <param name="controller">Controller to be added</param>
        public void AddController(IPanelStackController controller)
        {
            stackControllers.Add(controller);
        }

        /// <summary>
        /// Remove a panel stack Controller from list of stack event subscribers
        /// </summary>
        /// <param name="controller">Controller to be removed</param>
        public void RemoveController(IPanelStackController controller)
        {
            stackControllers.Remove(controller);
        }
        
        /// <summary>
        /// Push panel options onto top of panel stack
        /// </summary>
        /// <param name="options"></param>
        public async void Push(PushPanelOptions options)
        {
            await PushAsync(options);
        }

        /// <summary>
        /// Push panel options onto top of panel stack
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Task that completes when panel is done being pushed</returns>
        public async Task PushAsync(PushPanelOptions options)
        {
            stack.Add(options);

            if (stackControllers.Count == 1)
            {
                await stackControllers[0].TransitionAsync();
            }
            else
            {
                var tasks = new Task[stackControllers.Count];
            
                for (var i = 0; i < stackControllers.Count; i++)
                {
                    tasks[i] = stackControllers[i].TransitionAsync();
                }

                await Task.WhenAll(tasks);
            }
        }

        public async void Pop()
        {
            await PopAsync();
        }

        public async Task PopAsync()
        {
            stack.RemoveAt(stack.Count - 1);

            if (stackControllers.Count == 1)
            {
                await stackControllers[0].TransitionAsync();
            }
            else
            {
                var tasks = new Task[stackControllers.Count];
            
                for (var i = 0; i < stackControllers.Count; i++)
                {
                    tasks[i] = stackControllers[i].TransitionAsync();
                }

                await Task.WhenAll(tasks);
            }
        }
        
    }
}

