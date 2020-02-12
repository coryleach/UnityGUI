using System.Collections;
using System.Collections.Generic;
using Gameframe.GUI.PanelSystem;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Gameframe.GUI.Tests.Runtime
{
    public class AnimatedPanelViewTests
    {
        public void Test()
        {
            var gameObject = new GameObject();
            var animator = gameObject.AddComponent<Animator>();
            var animatedPanelView = gameObject.AddComponent<AnimatedPanelView>(); 
            var animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Packages/com.gameframe.gui/Animations/");
        }
    }
}

