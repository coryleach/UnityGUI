using UnityEngine;

namespace Gameframe.GUI
{
  /// <summary>
  /// This script modifies the camera projection to get an oblique camera angle
  /// </summary>
  public class ObliqueCamera : MonoBehaviour
  {
    
    [SerializeField]
    private float horizontal = 0;
    public float Horizontal
    {
      get => horizontal;
      set => horizontal = value;
    }
    
    [SerializeField]
    private float vertical = 0;
    public float Vertical
    {
      get => vertical;
      set => vertical = value;
    }
    
    public Matrix4x4 matrix;

    private UnityEngine.Camera _myCamera;
    public UnityEngine.Camera MyCamera
    {
      get
      {
        if (_myCamera == null)
        {
          _myCamera = GetComponent<UnityEngine.Camera>();
        }
        return _myCamera;
      }
    }

    private void OnEnable()
    {
      SetObliqueness(horizontal, vertical);
    }

    private void SetObliqueness(float horizObl, float vertObl)
    {
      var mat = UnityEngine.Camera.main.projectionMatrix;
      mat[0, 2] = horizObl;
      mat[1, 2] = vertObl;
      MyCamera.projectionMatrix = mat;
    }

    [ContextMenu("Reset Matrix")]
    private void ResetMatrix()
    {
      MyCamera.ResetProjectionMatrix();
      matrix = MyCamera.projectionMatrix;
    }

    private void OnValidate()
    {
      MyCamera.projectionMatrix = matrix;
    }

  }

}
