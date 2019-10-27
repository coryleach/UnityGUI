using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraOrthographicWidth : MonoBehaviour
    {

        [SerializeField]
        float desiredWidth = 6;

        [SerializeField]
        float minSize = 6;

        // Use this for initialization
        void OnEnable()
        {
            Refresh();
        }

        private void OnValidate()
        {
            Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            var cameraComponent = GetComponent<UnityEngine.Camera>();
            var height = desiredWidth / cameraComponent.aspect;
            cameraComponent.orthographicSize = Mathf.Max(height * 0.5f,minSize);
        }
    }
}


