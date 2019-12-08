using System.Threading.Tasks;
using Gameframe.GUI.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameframe.GUI.TransitionSystem
{
    public class SceneTransitionTask : ITransitionTask
    {
        public string[] unloadScenes;
        public string[] loadScenes;
        public float Progress { get; private set; }

        public async Task ExecuteAsync()
        {
            var unloadTasks = ListPool<AsyncOperation>.Get();
            var loadTasks = ListPool<AsyncOperation>.Get();
            
            Progress = 0;
            
            int totalScenes = unloadScenes.Length + loadScenes.Length;
            
            //Start Unloads
            for (var index = 0; index < unloadScenes.Length; index++)
            {
                var sceneName = unloadScenes[index];
                var async = SceneManager.UnloadSceneAsync(sceneName);
                unloadTasks.Add(async);
            }

            //Start Loads
            for (var index = 0; index < loadScenes.Length; index++)
            {
                var sceneName = loadScenes[index];
                var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                //Allow them all to load till 90% complete
                async.allowSceneActivation = false;
                loadTasks.Add(async);
            }

            var waiting = false;
            
            do
            {
                await Task.Yield();

                float sumProgress = 0;

                for (var index = 0; index < unloadTasks.Count; index++)
                {
                    var unload = unloadTasks[index];
                    if (!unload.isDone)
                    {
                        waiting = true;
                    }
                    sumProgress += unload.progress;
                }

                for (var index = 0; index < loadTasks.Count; index++)
                {
                    //Wait for all loads to reach 90% so we can activate them all at once
                    var load = loadTasks[index];
                    if (load.progress < 0.9f)
                    {
                        waiting = true;
                    }
                }

                Progress = sumProgress / totalScenes;

            } while (waiting);

            //Activate Scenes
            for (var index = 0; index < loadTasks.Count; index++)
            {
                var load = loadTasks[index];
                load.allowSceneActivation = true;
                while (!load.isDone)
                {
                    await Task.Yield();
                }
            }

            ListPool<AsyncOperation>.Release(unloadTasks);
            ListPool<AsyncOperation>.Release(loadTasks);

            //Load should now be complete
            Progress = 1f;
        }
    }
}