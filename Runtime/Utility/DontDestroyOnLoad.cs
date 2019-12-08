using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.Utility
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

