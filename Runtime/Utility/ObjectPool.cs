using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.Utility
{

  public class ObjectPool<T> where T : new()
  {
    readonly Stack<T> stack = new Stack<T>();
    readonly UnityAction<T> onGet;
    readonly UnityAction<T> onRelease;

    public int CountAll { get; private set; }
    public int CountActive { get { return CountAll - CountInactive; } }
    public int CountInactive { get { return stack.Count; } }

    public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
    {
      onGet = actionOnGet;
      onRelease = actionOnRelease;
    }

    public T Get()
    {
      T element;

      if (stack.Count == 0)
      {
        element = new T();
        CountAll++;
      }
      else
      {
        element = stack.Pop();
      }

      if (onGet != null)
      {
        onGet(element);
      }

      return element;
    }

    public void Release(T element)
    {
      if (stack.Count > 0 && ReferenceEquals(stack.Peek(), element))
      {
        Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
      }

      if (onRelease != null)
      {
        onRelease(element);
      }

      stack.Push(element);
    }

  }

}