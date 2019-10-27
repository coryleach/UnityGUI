using Gameframe.GUI;
using UnityEngine;
using CameraType = Gameframe.GUI.CameraType;

namespace Gameframe.GUI.Camera.UI
{
  public class UIFollower : MonoBehaviour
  {
    [SerializeField]
    private CameraCollection cameraCollection = null;

    [SerializeField]
    private CameraType environmentCameraType = null;

    [SerializeField]
    private CameraType uiCameraType = null;

    [SerializeField]
    private Transform target;
    public Transform Target
    {
      get => target;
      set { target = value; enabled = true; }
    }

    private RectTransform _rectTransform = null;
    private CameraDirector _environmentCameraDirector = null;
    private CameraDirector _uiCameraDirector = null;

    private void Start()
    {
      _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
      _environmentCameraDirector = cameraCollection.Get(environmentCameraType);
      _uiCameraDirector = cameraCollection.Get(uiCameraType);
    }

    private void OnDisable()
    {
      _environmentCameraDirector = null;
      _uiCameraDirector = null;
    }

    private void Update()
    {
      if (target == null)
      {
        enabled = false;
        return;
      }

      if (_environmentCameraDirector == null )
      {
        _environmentCameraDirector = cameraCollection.Get(environmentCameraType);
        if (_environmentCameraDirector == null)
        {
          Debug.LogWarningFormat(gameObject,"Failed to environment camera director");
          enabled = false;
          return;
        }
      }
    
      if ( _uiCameraDirector == null )
      {
        _uiCameraDirector = cameraCollection.Get(uiCameraType);
        if (_uiCameraDirector == null)
        {
          Debug.LogWarning("Failed to environment camera director");
          enabled = false;
          return;
        }
      }

      var screenPt = RectTransformUtility.WorldToScreenPoint(_environmentCameraDirector.Camera, target.position);
      if ( RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform, screenPt, _uiCameraDirector.Camera,out var targetPoint) )
      {
        _rectTransform.anchoredPosition = targetPoint;
      }
   
      //var pt = rectTransform.parent.InverseTransformPoint(targetPoint);
    }

  }
}
