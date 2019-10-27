using System.Collections.Generic;

namespace Gameframe.GUI.Utility
{

  public static class ListPool<T>
  {
    // Object pool to avoid allocations.
    static readonly ObjectPool<List<T>> listPool = new ObjectPool<List<T>>(null, l => l.Clear());

    public static List<T> Get()
    {
      return listPool.Get();
    }

    public static void Release(List<T> toRelease)
    {
      listPool.Release(toRelease);
    }
  }

}