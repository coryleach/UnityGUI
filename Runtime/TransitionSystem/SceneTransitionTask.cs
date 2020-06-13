using System;
using System.Threading.Tasks;
using Gameframe.GUI.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameframe.GUI.TransitionSystem
{
    public class SingleSceneTransitionTask : ITransitionTask
    {
        public string SceneName { get; set; } = string.Empty;
        public float Progress { get; private set; }
        public LoadSceneMode Mode { get; set; }  = LoadSceneMode.Single;

        public async Task ExecuteAsync()
        {
            var loadTasks = ListPool<AsyncOperation>.Get();

            //Start Loads
            var loadOperation = SceneManager.LoadSceneAsync(SceneName, Mode);
            
            if (loadOperation == null)
            {
                return;
            }
            
            //Allow them all to load till 90% complete
            loadOperation.allowSceneActivation = false;

            while (loadOperation.progress < 0.9f)
            {
                Progress = loadOperation.progress;
                await Task.Yield();
            }

            loadOperation.allowSceneActivation = true;
            
            while (!loadOperation.isDone)
            {
                Progress = loadOperation.progress;
                await Task.Yield();
            }
            
            ListPool<AsyncOperation>.Release(loadTasks);

            //Load should now be complete
            Progress = 1f;

            //Yield one last frame in case to allow 100% progress to be handled by presenter
            await Task.Yield();
        }
    }
    
    public class MultiSceneTransitionTask : ITransitionTask
    {
        public string[] UnloadScenes { get; set; }
        public string[] LoadScenes { get; set; }
        public float Progress { get; private set; }
        public LoadSceneMode Mode { get; set; } = LoadSceneMode.Additive;

        public async Task ExecuteAsync()
        {
            var unloadTasks = ListPool<AsyncOperation>.Get();
            var loadTasks = ListPool<AsyncOperation>.Get();
            
            Progress = 0;
            
            int totalScenes = UnloadScenes.Length + LoadScenes.Length;
            
            //Start Unloads
            for (var index = 0; index < UnloadScenes.Length; index++)
            {
                var sceneName = UnloadScenes[index];
                var unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
                if (unloadOperation != null)
                {
                    unloadTasks.Add(unloadOperation);
                }
                else
                {
                    Debug.LogError($"Failed to unload scene {sceneName}. UnloadSceneAsync returned null.");
                }
            }

            //Start Loads
            for (var index = 0; index < LoadScenes.Length; index++)
            {
                var sceneName = LoadScenes[index];
                var asyncOperation = SceneManager.LoadSceneAsync(sceneName, Mode);
                if (asyncOperation != null)
                {
                    //Allow them all to load till 90% complete
                    asyncOperation.allowSceneActivation = false;
                    loadTasks.Add(asyncOperation);
                }
                else
                {
                    Debug.LogError($"Failed to load scene {sceneName}. LoadSceneAsync returned null.");
                }
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
            
            //Yielding one last time to let the 100% progress to be handled by presenter
            await Task.Yield();
        }
    }
}