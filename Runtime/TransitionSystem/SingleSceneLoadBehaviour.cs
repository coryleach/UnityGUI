using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.TransitionSystem
{
    public class SingleSceneLoadBehaviour : MonoBehaviour
    {
        public SceneTransitionSystem sceneTransitionSystem;
        public string sceneName;
        
        [ContextMenu("Load")]
        public void Load()
        {
            sceneTransitionSystem.LoadScene(sceneName);
        }
    }
}


