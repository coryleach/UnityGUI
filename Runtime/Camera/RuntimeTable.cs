using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI
{
  public class RuntimeTable<TKey, TValue> : ScriptableObject
  {
    [SerializeField]
    private bool allowOverwrite = true;

    [SerializeField]
    private bool throwExceptions = false;

    private readonly Dictionary<TKey, TValue> _items = new Dictionary<TKey, TValue>();

    public IReadOnlyDictionary<TKey, TValue> Items => _items;

    public TValue Get(TKey key)
    {
      if ( throwExceptions )
      {
        return _items[key];
      }

      if ( _items.TryGetValue(key,out var val))
      {
        return val;
      }
      
      return default(TValue);
    }

    public void Add(TKey key, TValue val)
    {
      if ( allowOverwrite )
      {
        _items[key] = val;
      }
      else
      {
        _items.Add(key, val);
      }
    }

    public bool Remove(TKey key)
    {
      return _items.Remove(key);
    }

    protected virtual void OnEnable()
    {
      _items.Clear();
    }
  }
}