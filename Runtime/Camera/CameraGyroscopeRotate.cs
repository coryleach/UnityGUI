namespace Gameframe.GUI
{
  using UnityEngine;

  public class CameraGyroscopeRotate : MonoBehaviour
  {
    [SerializeField]
    Vector3 minDelta;

    [SerializeField]
    Vector3 maxDelta;

    [SerializeField]
    Vector3 sensitivity = new Vector3(1f, 1f, 0f);

    [SerializeField]
    float deadZone = 0.15f;

    [SerializeField]
    float smoothTime = 1f;

    float velocityX = 0;
    float velocityY = 0;
    float velocityZ = 0;

    float delay = 0.5f;

    Vector3 startRotation;

    void Start()
    {
      startRotation = transform.eulerAngles;
      if (!SystemInfo.supportsGyroscope)
      {
        enabled = false;
        return;
      }
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

    void LateUpdate()
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
