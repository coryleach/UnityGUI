using UnityEngine;

namespace Gameframe.GUI.Layout
{
    public class SafeAreaLayout : MonoBehaviour
    {
        private RectTransform _safeAreaRect = null;

        [SerializeField]
        private Canvas _canvas = null;

#if UNITY_EDITOR
        private static bool debug = false;
#endif

        private void Awake()
        {
            if (_canvas == null)
            {
                _canvas = GetComponentInParent<Canvas>().rootCanvas;
            }
            _safeAreaRect = transform as RectTransform;
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void OnRectTransformDimensionsChange()
        {
            Refresh();
        }

        [ContextMenu("Refresh")]
        private void Refresh()
        {
            if (_safeAreaRect == null)
            {
                return;
            }

            var safeArea = GetSafeArea();
            var pixelRect = _canvas.pixelRect;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;

            _safeAreaRect.anchorMin = anchorMin;
            _safeAreaRect.anchorMax = anchorMax;
        }

        private Rect GetSafeArea()
        {
#if UNITY_EDITOR
            var safeArea = Screen.safeArea;

            if (debug)
            {
                safeArea.y += 100;
                safeArea.height -= 100;
                safeArea.height -= 100;
            }

            return safeArea;
#else
        return Screen.safeArea;
#endif
        }
    }
}

