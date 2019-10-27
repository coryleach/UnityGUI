using UnityEngine;

namespace Gameframe.GUI.Extensions
{

  public static class MonobehaviourExt
  {

    static public T GetOrAddComponent<T>(this MonoBehaviour obj) where T : Component
    {
      T result = obj.GetComponent<T>();
      if (result == null)
      {
        result = obj.gameObject.AddComponent<T>();
      }
      return result;
    }

  }

}