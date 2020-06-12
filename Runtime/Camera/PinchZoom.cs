using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameframe.GUI
{

  public class PinchZoom : MonoBehaviour
  {

    [SerializeField]
    private float zoomSpeed = 0.5f;

    [SerializeField]
    private float minimumZoom = 3f;
    
    [SerializeField]
    private float maximumZoom = 100f;

    [SerializeField]
    private float mouseScrollSensitivity = 1f;

    [SerializeField]
    private float minimumStretchZoom = 3f;
    
    [SerializeField]
    private float maximumStretchZoom = 102f;

    private UnityEngine.Camera _camera;

    private void Start()
    {
      _camera = GetComponent<UnityEngine.Camera>();

      minimumStretchZoom = minimumZoom - 2f;
      if ( minimumStretchZoom < 0 )
      {
        minimumStretchZoom = minimumZoom;
      }

      maximumStretchZoom = maximumZoom + 2;
    }

    private void Update()
    {
      if ( EventSystem.current == null || EventSystem.current.IsPointerOverGameObject() )
      {
        return;
      }

      if ( _camera.orthographicSize < this.minimumZoom && Input.touchCount == 0 )
      {
        _camera.orthographicSize = Mathf.Lerp( _camera.orthographicSize, this.minimumZoom, 0.1f );
      }

      if ( _camera.orthographicSize > this.maximumZoom && Input.touchCount == 0 )
      {
        _camera.orthographicSize = Mathf.Lerp( _camera.orthographicSize, this.maximumZoom, 0.1f );
      }

      if ( Input.touchCount == 2 )
      {
        var touch0 = Input.touches[ 0 ];
        var touch1 = Input.touches[ 1 ];

        //Get Previous Positions
        var prevTouch0 = touch0.position - touch0.deltaPosition;
        var prevTouch1 = touch1.position - touch1.deltaPosition;

        //Get Previous Distance
        var prevDistance = ( prevTouch0 - prevTouch1 ).magnitude;
        var touchDistance = ( touch0.position - touch1.position ).magnitude;

        var diff = prevDistance - touchDistance;

        if ( _camera.orthographic )
        {

          _camera.orthographicSize += diff * zoomSpeed;

          //Clamp Zoom
          _camera.orthographicSize = Mathf.Clamp( _camera.orthographicSize, this.minimumStretchZoom, this.maximumStretchZoom );

        }
        else
        {

          _camera.fieldOfView += diff * zoomSpeed;

          //Clamp Zoom
          _camera.fieldOfView = Mathf.Clamp( _camera.fieldOfView, this.minimumStretchZoom, this.maximumStretchZoom );

        }

      }
      else if ( !Mathf.Approximately(Input.GetAxis( "Mouse ScrollWheel" ), 0) )
      {

        if ( _camera.orthographic )
        {

          _camera.orthographicSize += Input.GetAxis( "Mouse ScrollWheel" ) * mouseScrollSensitivity * zoomSpeed;

          //Clamp Zoom
          _camera.orthographicSize = Mathf.Clamp( _camera.orthographicSize, this.minimumStretchZoom, this.maximumStretchZoom );

        }
        else
        {

          _camera.fieldOfView += Input.GetAxis( "Mouse ScrollWheel" ) * mouseScrollSensitivity * zoomSpeed;

          //Clamp Zoom
          _camera.fieldOfView = Mathf.Clamp( _camera.fieldOfView, this.minimumStretchZoom, this.maximumStretchZoom );

        }

      }

    }


  }

}