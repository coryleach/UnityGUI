using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.TransitionSystem
{
    public class SingleSceneLoadBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionSystem sceneTransitionSystem;
        
        [SerializeField]
        private string sceneName;
        public string SceneName
        {
            get => sceneName;
            set => sceneName = value;
        }
        
        [ContextMenu("Load")]
        public void Load()
        {
            sceneTransitionSystem.LoadScene(sceneName);
        }
    }
}


