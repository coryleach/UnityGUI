using UnityEngine;

namespace Gameframe.GUI.TransitionSystem
{
    public class MultiSceneLoadBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionSystem sceneTransitionSystem;
        
        [SerializeField]
        private string[] unloadScenes = new string[0];
        
        [SerializeField]
        private string[] loadScenes = new string[0];
        
        [ContextMenu("Load")]
        public void Load()
        {
            sceneTransitionSystem.LoadScene(loadScenes, unloadScenes);
        }
    }   
}

