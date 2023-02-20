using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.Utility
{
    /// <summary>
    /// Add this component to quickly mark any GameObject as DontDestroyOnLoad
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

