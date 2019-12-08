using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameframe.GUI.Utility;
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
        
        private bool isTransitioning = false;
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

        public async void LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (isTransitioning)
            {
                throw new InvalidOperationException("Cannot load scenes while scene transition is in progress");
            }
            
            singleSceneTransitionTask.mode = mode;
            singleSceneTransitionTask.sceneName = sceneName;
            
            isTransitioning = true;
            transition.AddTransitionTask(singleSceneTransitionTask);
            await transition.ExecuteAsync();
            transition.RemoveTransitionTask(singleSceneTransitionTask);
            isTransitioning = false;
        }
        
        public async void LoadSceneAsync(string[] loadScenes, string[] unloadScenes)
        {
            if (isTransitioning)
            {
                throw new InvalidOperationException("Cannot load scenes while scene transition is in progress");
            }
            
            isTransitioning = true;
            multiSceneTransitionTask.loadScenes = loadScenes;
            multiSceneTransitionTask.unloadScenes = unloadScenes;
            transition.AddTransitionTask(multiSceneTransitionTask);
            await transition.ExecuteAsync();
            transition.RemoveTransitionTask(multiSceneTransitionTask);
            isTransitioning = false;
        }

        public async void LoadSceneAsync(string[] loadScenes)
        {
            if (isTransitioning)
            {
                throw new InvalidOperationException("Cannot load scenes while scene transition is in progress");
            }
            
            isTransitioning = true;
            multiSceneTransitionTask.loadScenes = loadScenes;
            multiSceneTransitionTask.unloadScenes = new string[0];
            transition.AddTransitionTask(multiSceneTransitionTask);
            await transition.ExecuteAsync();
            transition.RemoveTransitionTask(multiSceneTransitionTask);
            isTransitioning = false;
        }
        
    }
}
