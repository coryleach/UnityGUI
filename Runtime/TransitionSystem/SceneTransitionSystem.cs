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
        public Transition Transition => transition;
        
        private SceneTransitionTask sceneTransitionTask = new SceneTransitionTask();
        
        private bool isTransitioning = false;
        public bool IsTransitioning => isTransitioning;
        
        private void OnEnable()
        {
            transition = new Transition();
            sceneTransitionTask = new SceneTransitionTask();
            transition.AddTransitionTask(sceneTransitionTask);
        }

        public async void LoadSceneAsync(string[] unloadScenes, string[] loadScenes)
        {
            isTransitioning = true;
            sceneTransitionTask.loadScenes = loadScenes;
            sceneTransitionTask.unloadScenes = unloadScenes;
            await transition.ExecuteAsync();
            isTransitioning = false;
        }
        
    }
}
