using UnityEngine;

namespace Gameframe.GUI.Extensions
{

  public static class GameObjectExt
  {

    public static void SetLayerRecursively( this GameObject obj, int layer )
    {
      obj.layer = layer;
      foreach ( Transform child in obj.transform )
      {
        child.gameObject.SetLayerRecursively( layer );
      }
    }

    static public T GetOrAddComponent<T>( this GameObject obj ) where T : Component
    {
      T result = obj.GetComponent<T>();
      if ( result == null )
      {
        result = obj.AddComponent<T>();
      }
      
      return result;
    }

  }

}