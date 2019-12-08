using System.Collections.Generic;
using System.Threading.Tasks;
using Gameframe.GUI.Utility;

namespace Gameframe.GUI.TransitionSystem
{
        public class Transition
        {
            private List<ITransitionTask> transitionTasks = new List<ITransitionTask>();
            private List<ITransitionPresenter> transitionPresenters = new List<ITransitionPresenter>();
    
            public void AddPresenter(ITransitionPresenter presenter)
            {
                transitionPresenters.Add(presenter);
            }
    
            public void RemovePresenter(ITransitionPresenter presenter)
            {
                transitionPresenters.Remove(presenter);
            }
    
            public void AddTransitionTask(ITransitionTask transitionTask)
            {
                transitionTasks.Add(transitionTask);
            }
    
            public void RemoveTransitionTask(ITransitionTask transitionTask)
            {
                transitionTasks.Add(transitionTask);
            }
    
            /// <summary>
            /// Executes transition
            /// If you want to await this operation use TransitionAsync
            /// </summary>
            public async void Execute()
            {
                await ExecuteAsync();
            }
            
            /// <summary>
            /// Awaitable Transition
            /// </summary>
            /// <returns>Task that completes when transition is complete</returns>
            public async Task ExecuteAsync()
            {
                var tasks = ListPool<Task>.Get();
    
                //Start Transition Presenters
                for (var index = 0; index < transitionPresenters.Count; index++)
                {
                    var presenter = transitionPresenters[index];
                    tasks.Add(presenter.StartTransitionAsync());
                }
                await Task.WhenAll(tasks);
                
                //Do Transition Tasks
                tasks.Clear();
                foreach (var transitionTask in transitionTasks)
                {
                    tasks.Add(transitionTask.ExecuteAsync());
                }
                await Task.WhenAll(tasks);
                
                //Finish Transition Presenters
                tasks.Clear();
                for (var index = 0; index < transitionPresenters.Count; index++)
                {
                    var presenter = transitionPresenters[index];
                    tasks.Add(presenter.FinishTransitionAsync());
                }
    
                await Task.WhenAll(tasks);
            }
        }
}