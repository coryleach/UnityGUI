using UnityEngine;

namespace Gameframe.GUI.TransitionSystem
{
    public class MultiSceneLoadBehaviour : MonoBehaviour
    {
        public SceneTransitionSystem sceneTransitionSystem;
        public string[] unloadScenes = new string[0];
        public string[] loadScenes = new string[0];
        
        [ContextMenu("Load")]
        public void Load()
        {
            sceneTransitionSystem.LoadSceneAsync(loadScenes, unloadScenes);
        }
    }   
}

