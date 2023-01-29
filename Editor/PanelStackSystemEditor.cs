using System;
using UnityEditor;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
  [CustomEditor(typeof(ScriptablePanelStackSystem))]
  public class PanelStackSystemEditor : UnityEditor.Editor
  {
    public override bool RequiresConstantRepaint()
    {
      return true;
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.BeginVertical("box");
      EditorGUILayout.LabelField("Current Stack");
      
      var stack = (ScriptablePanelStackSystem) target;
      for (int i = stack.Count-1; i >= 0; i--)
      {
        EditorGUILayout.BeginVertical("box");
        var controller = stack[i];
        if (controller == null)
        {
          EditorGUILayout.LabelField($"{i}: Missing Panel Controller");
        }
        else if (controller.PanelType == null)
        {
          EditorGUILayout.LabelField($"{i}: Unknown Panel");
        }
        else
        {
          EditorGUILayout.LabelField($"{i}: {controller.PanelType.name}");
          EditorGUILayout.LabelField($"Loaded: {controller.IsViewLoaded} Visible: {controller.IsViewLoaded && controller.View.gameObject.activeSelf}");
        }
        EditorGUILayout.EndVertical();
      }
      
      EditorGUILayout.EndVertical();
    }
  }
}


