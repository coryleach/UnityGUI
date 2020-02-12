using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Gameframe.GUI.Editor
{
    public class GUIEditor : UnityEditor.Editor
    {

        [MenuItem("Assets/Nest Animation Clips")]
        public static void NestAnimationClips()
        {
            var animatorControllers = Selection.objects.OfType<AnimatorController>().ToList();

            if (animatorControllers.Count > 1)
            {
                Debug.LogError("More than one AnimatorController was selected. Please select only one AnimtorController.");
                return;
            }

            if (animatorControllers.Count == 0)
            {
                Debug.LogError("You must select exactly one AnimatorController asset together with your animation clips.");
                return;
            }

            var controller = animatorControllers[0];
            if (controller == null)
            {
                Debug.Log("Failed to get animator controller from selection list.");
                return;
            }

            var clips = Selection.objects.OfType<AnimationClip>().ToList();

            if (clips.Count == 0)
            {
                return;
            }
            
            foreach (var oldClip in clips)
            {
                var newClip = Instantiate(oldClip);
                newClip.name = oldClip.name;
                AssetDatabase.AddObjectToAsset(newClip, controller);
                Debug.Log("Added & Will Import: " + AssetDatabase.GetAssetPath(newClip));
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newClip));
                //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oldClip));
            }
        }
        
    }

}