namespace Gameframe.GUI
{
  using UnityEngine;

  public class CameraGyroscopeRotate : MonoBehaviour
  {
    [SerializeField] 
    private Vector3 minDelta = Vector3.zero;

    [SerializeField] 
    private Vector3 maxDelta = Vector3.zero;

    [SerializeField] 
    private Vector3 sensitivity = new Vector3(1f, 1f, 0f);

    [SerializeField] 
    private float deadZone = 0.15f;

    [SerializeField] 
    private float smoothTime = 1f;

    private float velocityX;
    private float velocityY;
    private float velocityZ;

    private float delay = 0.5f;

    private Vector3 startRotation;

    private void Start()
    {
      if (!SystemInfo.supportsGyroscope)
      {
        enabled = false;
        return;
      }
      startRotation = transform.eulerAngles;
    }

    private void OnEnable()
    {
      if (Input.gyro != null)
      {
        Input.gyro.enabled = true;
        delay = 1f;
      }
    }

    private void OnDisable()
    {
      if (Input.gyro != null)
      {
        Input.gyro.enabled = false;
      }
    }

    private void LateUpdate()
    {
      //The gyro always has some junk data on enable so let everything initialize for a moment
      if (delay > 0)
      {
        delay -= Time.deltaTime;
      }

      var rotationRate = Input.gyro.rotationRate;

      if (Mathf.Abs(rotationRate.x) < deadZone)
      {
        rotationRate.x = 0;
      }

      if (Mathf.Abs(rotationRate.y) < deadZone)
      {
        rotationRate.y = 0;
      }

      if (Mathf.Abs(rotationRate.z) < deadZone)
      {
        rotationRate.z = 0;
      }

      rotationRate = rotationRate * Mathf.Rad2Deg * Time.deltaTime;

      rotationRate.x *= sensitivity.x;
      rotationRate.y *= sensitivity.y;
      rotationRate.z *= sensitivity.z;

      var target = transform.eulerAngles + rotationRate;

      target.x = Mathf.SmoothDampAngle(transform.eulerAngles.x, target.x, ref velocityX, smoothTime);
      target.y = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.y, ref velocityY, smoothTime);
      target.z = Mathf.SmoothDampAngle(transform.eulerAngles.z, target.z, ref velocityZ, smoothTime);

      target.x = Mathf.MoveTowardsAngle(startRotation.x, target.x, maxDelta.x);
      target.y = Mathf.MoveTowardsAngle(startRotation.y, target.y, maxDelta.y);
      target.z = Mathf.MoveTowardsAngle(startRotation.z, target.z, maxDelta.z);

      transform.eulerAngles = target;

      //Slowly move back to center
      transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(startRotation), Time.deltaTime * 0.1f);
    }

  }

}
