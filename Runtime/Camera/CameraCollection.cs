using Gameframe.GUI.Camera;
using UnityEngine;

namespace Gameframe.GUI
{
  [CreateAssetMenu(menuName = "Gameframe/Cameras/CameraCollection")]
  public class CameraCollection : RuntimeTable<CameraType, CameraDirector>
  {
  }
}