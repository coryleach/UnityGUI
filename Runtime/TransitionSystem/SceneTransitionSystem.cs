using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameframe.GUI.TransitionSystem
{
    [CreateAssetMenu(menuName="Gameframe/SceneTransitionSystem")]
    public class SceneTransitionSystem : ScriptableObject
    {
        private Transition transition = new Transition();

        private SingleSceneTransitionTask singleSceneTransitionTask = new SingleSceneTransitionTask();
        private MultiSceneTransitionTask multiSceneTransitionTask = new MultiSceneTransitionTask();

        private bool isTransitioning;
        public bool IsTransitioning => isTransitioning;

        public void AddPresenter(ITransitionPresenter presenter)
        {
            transition.AddPresenter(presenter);
        }

        public void RemovePresenter(ITransitionPresenter presenter)
        {
            transition.RemovePresenter(presenter);
        }

        private void OnEnable()
        {
            isTransitioning = false;
            transition = new Transition();
            multiSceneTransitionTask = new MultiSceneTransitionTask();
            singleSceneTransitionTask = new SingleSceneTransitionTask();
        }

        public async void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            await LoadSceneAsync(sceneName, mode);
        }

        public async Task LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (isTransitioning)
            {
                throw new InvalidOperationException("Cannot load scenes while scene transition is in progress");
            }

            singleSceneTransitionTask.Mode = mode;
            singleSceneTransitionTask.SceneName = sceneName;

            isTransitioning = true;
            transition.AddTransitionTask(singleSceneTransitionTask);
            await transition.ExecuteAsync();
            transition.RemoveTransitionTask(singleSceneTransitionTask);
            isTransitioning = false;
        }

        public async void LoadScene(string[] loadScenes, string[] unloadScenes)
        {
            await LoadScenesAsync(loadScenes, unloadScenes);
        }

        [Obsolete("Use LoadScenesAsync method instead", false)]
        public Task LoadSceneAsync(string[] loadScenes, string[] unloadScenes)
        {
            return LoadScenesAsync(loadScenes,unloadScenes);
        }

        /// <summary>
        /// Unload and Load multiple scenes
        /// </summary>
        /// <param name="loadScenes">scenes to load</param>
        /// <param name="unloadScenes">scenes to unload</param>
        /// <returns>Task that completes when the load is finished</returns>
        /// <exception cref="InvalidOperationException">Throws invalid operation exception if a transition is already in progress</exception>
        public async Task LoadScenesAsync(string[] loadScenes, string[] unloadScenes)
        {
            if (isTransitioning)
            {
                throw new InvalidOperationException("Cannot load scenes while scene transition is in progress");
            }

            isTransitioning = true;
            multiSceneTransitionTask.LoadScenes = loadScenes;
            multiSceneTransitionTask.UnloadScenes = unloadScenes;
            transition.AddTransitionTask(multiSceneTransitionTask);
            await transition.ExecuteAsync();
            transition.RemoveTransitionTask(multiSceneTransitionTask);
            isTransitioning = false;
        }

        public async void LoadScenes(string[] scenesToLoad)
        {
            await LoadScenesAsync(scenesToLoad);
        }

        public async Task LoadScenesAsync(string[] loadScenes)
        {
            if (isTransitioning)
            {
                throw new InvalidOperationException("Cannot load scenes while scene transition is in progress");
            }

            isTransitioning = true;
            multiSceneTransitionTask.LoadScenes = loadScenes;
            multiSceneTransitionTask.UnloadScenes = new string[0];
            transition.AddTransitionTask(multiSceneTransitionTask);
            await transition.ExecuteAsync();
            transition.RemoveTransitionTask(multiSceneTransitionTask);
            isTransitioning = false;
        }

    }
}
