namespace Gameframe.GUI
{

  using UnityEngine;

  public class ObliqueCamera : MonoBehaviour
  {

    public float horizontal = 0;
    public float vertical = 0;

    public Matrix4x4 matrix;

    UnityEngine.Camera myCamera;
    public UnityEngine.Camera MyCamera
    {
      get
      {
        if (myCamera == null)
        {
          myCamera = GetComponent<UnityEngine.Camera>();
        }
        return myCamera;
      }
    }

    void OnEnable()
    {
      SetObliqueness(horizontal, vertical);
    }

    void SetObliqueness(float horizObl, float vertObl)
    {
      Matrix4x4 mat = UnityEngine.Camera.main.projectionMatrix;
      mat[0, 2] = horizObl;
      mat[1, 2] = vertObl;
      MyCamera.projectionMatrix = mat;
    }

    [ContextMenu("Reset Matrix")]
    void ResetMatrix()
    {
      MyCamera.ResetProjectionMatrix();
      matrix = MyCamera.projectionMatrix;
    }

    void OnValidate()
    {
      MyCamera.projectionMatrix = matrix;
    }

  }

}
