using System;
using UnityEngine;

namespace Gameframe.GUI
{
    /// <summary>
    /// This component allows you to align one rect transform to a point or corner on another rectTransform.
    /// It can also keep the bounds of the attached rect within the compoent's rectTransform bounds.
    /// If the attachment rect overlaps in a way that goes out of bounds the attachment will jumpt to the opposite side.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIAttachmentView : MonoBehaviour
    {
        
        public enum Location
        {
            TopLeft,
            TopMiddle,
            TopRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            BottomLeft,
            BottomMiddle,
            BottomRight
        }

        [SerializeField] 
        private RectTransform _target = null;

        [SerializeField] 
        private RectTransform _attachment = null;

        [SerializeField] 
        private Location locationOnTarget = Location.MiddleCenter;

        [SerializeField] 
        private bool rotate = true;

        [SerializeField, Tooltip("Will keep the child attachment within the bounds parent rect by flipping the attachment position.")] 
        private bool keepInView = false;

        [SerializeField, Tooltip("The distance the attachment can get to an edge before flipping to be kept in view")] 
        private float padding = 100;

        public float Padding
        {
            get => padding;
            set => padding = value;
        }

        private Canvas _parentCanvas = null;
        
        private RectTransform _myRectTransform = null;

        private void Awake()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            _myRectTransform = (RectTransform) transform;
            if (_attachment != null)
            {
                if (_attachment.parent != _myRectTransform)
                {
                    _attachment.SetParent(_myRectTransform);
                }
                _attachment.gameObject.SetActive(false);
            }
        }

        [ContextMenu("Show")]
        public void Show()
        {
            if (_attachment != null)
            {
                _attachment.gameObject.SetActive(true);
            }
            Refresh();
        }
        
        public void Show(RectTransform target, Location location = Location.TopMiddle)
        {
            locationOnTarget = location;
            _target = target;

            if (_attachment != null)
            {
                _attachment.gameObject.SetActive(true);
            }

            Refresh();
        }

        [ContextMenu("Dismiss")]
        public void Dismiss()
        {
            if (_attachment == null)
            {
                return;
            }

            _attachment.gameObject.SetActive(false);
        }

        public void Refresh()
        {
            if (_target == null || _attachment == null)
            {
                return;
            }

            var parentRect = _myRectTransform.rect;
            var location = locationOnTarget;
            var pointerPoint = GetPoint(location);

            if (keepInView)
            {
                //Calculate distance between point and the sides
                var distanceToTop = parentRect.max.y - pointerPoint.y;
                var distanceToBottom = pointerPoint.y - parentRect.min.y;
                var distanceToLeft = pointerPoint.x - parentRect.min.x;
                var distanceToRight = parentRect.max.x - pointerPoint.x;

                //Get pointer point within rect.
                if (distanceToTop < padding && IsTop(location))
                {
                    location = FlipHorizontal(location);
                    pointerPoint = GetPoint(location);
                }
                else if (distanceToBottom < padding && IsBottom(location))
                {
                    location = FlipHorizontal(location);
                    pointerPoint = GetPoint(location);
                }

                if (distanceToLeft < padding && IsLeft(location))
                {
                    location = FlipVertical(location);
                    pointerPoint = GetPoint(location);
                }
                else if (distanceToRight < padding && IsRight(location))
                {
                    location = FlipVertical(location);
                    pointerPoint = GetPoint(location);
                }
            }

            _attachment.anchoredPosition = pointerPoint;

            if (rotate)
            {
                _attachment.localRotation = GetRotation(location);
            }
        }

        private Vector3 GetPoint(UnityEngine.Camera camera, Vector3[] corners, Location location)
        {
            switch (location)
            {
                case Location.BottomLeft:
                    return RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
                case Location.MiddleLeft:
                    return RectTransformUtility.WorldToScreenPoint(camera, (corners[0] + corners[1]) * 0.5f);
                case Location.TopLeft:
                    return RectTransformUtility.WorldToScreenPoint(camera, corners[1]);
                case Location.BottomMiddle:
                    return RectTransformUtility.WorldToScreenPoint(camera, (corners[0] + corners[3]) * 0.5f);
                case Location.MiddleCenter:
                    return RectTransformUtility.WorldToScreenPoint(camera, (corners[0] + corners[2]) * 0.5f);
                case Location.TopMiddle:
                    return RectTransformUtility.WorldToScreenPoint(camera, (corners[1] + corners[2]) * 0.5f);
                case Location.BottomRight:
                    return RectTransformUtility.WorldToScreenPoint(camera, corners[3]);
                case Location.MiddleRight:
                    return RectTransformUtility.WorldToScreenPoint(camera, (corners[3] + corners[2]) * 0.5f);
                case Location.TopRight:
                    return RectTransformUtility.WorldToScreenPoint(camera, corners[2]);
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        private Quaternion GetRotation(Location location)
        {
            switch (location)
            {
                case Location.TopLeft:
                    return Quaternion.Euler(0, 0, 45);
                case Location.TopMiddle:
                    return Quaternion.identity;
                case Location.TopRight:
                    return Quaternion.Euler(0, 0, -45);
                case Location.MiddleLeft:
                    return Quaternion.Euler(0, 0, 90);
                case Location.MiddleCenter:
                    return Quaternion.identity;
                case Location.MiddleRight:
                    return Quaternion.Euler(0, 0, -90);
                case Location.BottomLeft:
                    return Quaternion.Euler(0, 0, 135);
                case Location.BottomMiddle:
                    return Quaternion.Euler(0, 0, 180);
                case Location.BottomRight:
                    return Quaternion.Euler(0, 0, -135);
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        private bool IsTop(Location location)
        {
            switch (location)
            {
                case Location.TopLeft:
                case Location.TopMiddle:
                case Location.TopRight:
                    return true;
                case Location.MiddleLeft:
                case Location.MiddleCenter:
                case Location.MiddleRight:
                case Location.BottomLeft:
                case Location.BottomMiddle:
                case Location.BottomRight:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        private bool IsBottom(Location location)
        {
            switch (location)
            {
                case Location.TopLeft:
                case Location.TopMiddle:
                case Location.TopRight:
                case Location.MiddleLeft:
                case Location.MiddleCenter:
                case Location.MiddleRight:
                    return false;
                case Location.BottomLeft:
                case Location.BottomMiddle:
                case Location.BottomRight:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        private bool IsLeft(Location location)
        {
            switch (location)
            {
                case Location.BottomMiddle:
                case Location.BottomRight:
                case Location.TopMiddle:
                case Location.TopRight:
                case Location.MiddleCenter:
                case Location.MiddleRight:
                    return false;
                case Location.TopLeft:
                case Location.MiddleLeft:
                case Location.BottomLeft:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        private bool IsRight(Location location)
        {
            switch (location)
            {
                case Location.TopLeft:
                case Location.TopMiddle:
                case Location.MiddleLeft:
                case Location.MiddleCenter:
                case Location.BottomLeft:
                case Location.BottomMiddle:
                    return false;
                case Location.MiddleRight:
                case Location.TopRight:
                case Location.BottomRight:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        private Vector2 GetPoint(Location location)
        {
            var pointerPoint = Vector2.zero;
            var corners = new Vector3[4];
            _target.GetWorldCorners(corners);

            //Check for a world space camera if we have one
            var worldCamera = _parentCanvas.worldCamera;

            //OK for camera to be null here if canvas render mode is screen space overlay
            var screenPoint = GetPoint(worldCamera, corners, location);

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_myRectTransform, screenPoint, worldCamera,
                out pointerPoint))
            {
                Debug.LogError("plane of the RectTransform was not hit");
                return Vector2.zero;
            }

            return pointerPoint;
        }

        private Location FlipHorizontal(Location location)
        {
            switch (location)
            {
                case Location.TopLeft:
                    return Location.BottomLeft;
                case Location.TopMiddle:
                    return Location.BottomMiddle;
                case Location.TopRight:
                    return Location.BottomRight;
                case Location.BottomLeft:
                    return Location.TopLeft;
                case Location.BottomMiddle:
                    return Location.TopMiddle;
                case Location.BottomRight:
                    return Location.TopRight;
                default:
                    return location;
            }
        }

        private Location FlipVertical(Location location)
        {
            switch (location)
            {
                case Location.TopLeft:
                    return Location.TopRight;
                case Location.TopRight:
                    return Location.TopLeft;
                case Location.MiddleLeft:
                    return Location.MiddleRight;
                case Location.MiddleRight:
                    return Location.MiddleLeft;
                case Location.BottomLeft:
                    return Location.BottomRight;
                case Location.BottomRight:
                    return Location.BottomLeft;
                default:
                    return location;
            }
        }
    }
}