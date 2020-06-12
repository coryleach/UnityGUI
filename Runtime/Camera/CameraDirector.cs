using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI
{
    using Camera = UnityEngine.Camera;

    /// <summary>
    /// Takes care of managing a set of components that control camera movement and behavior
    /// </summary>
    public class CameraDirector : MonoBehaviour
    {
        /// <summary>
        /// Collection to add this camera to on awake
        /// </summary>
        [SerializeField]
        private CameraCollection collection = null;

        /// <summary>
        /// Unity events that recieve CameraDirector as an argument
        /// </summary>
        [System.Serializable]
        public class CameraDirectorEvent : UnityEvent<CameraDirector>
        {
        }

        [SerializeField]
        private CameraType cameraType = null;
        public CameraType CameraType => cameraType;

        [SerializeField]
        UnityEngine.Camera _camera;
        public UnityEngine.Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponent<UnityEngine.Camera>();
                    Debug.Assert(_camera != null, "CameraDirector has no Camera component!");
                }

                return _camera;
            }
        }

        public bool canOverwriteExistingCamera = true;

        public CameraDirectorEvent OnDirectorEnabled { get; } = new CameraDirectorEvent();
        public CameraDirectorEvent OnDirectorDisabled { get; } = new CameraDirectorEvent();

        protected virtual void Awake()
        {
            if (!canOverwriteExistingCamera && collection?.Get(cameraType) != null)
            {
                //Camera type already exists in collection.
                return;
            }
            collection?.Add(cameraType, this);
        }

        protected virtual void OnEnable()
        {
            OnDirectorEnabled.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            OnDirectorDisabled.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            if (collection == null)
            {
                return;
            }

            if (collection.Items.TryGetValue(CameraType, out var val))
            {
                if (val == this)
                {
                    collection.Remove(cameraType);
                }
            }
        }
    }
}