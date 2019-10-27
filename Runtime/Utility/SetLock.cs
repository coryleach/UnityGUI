using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.Utility
{
  /// <summary>
  /// SetLock requires a 'requester' parameter when locking or unlocking
  /// Internally it uses a hash set so that the same requester cannot lock twice
  /// and even if locked twice by the same requester it only needs to be unlocked once by that requester to unlock
  /// </summary>
  public class SetLock : MonoBehaviour
  {

    HashSet<object> set = new HashSet<object>();

    public UnityEvent onUnlocked = new UnityEvent();
    public UnityEvent onLocked = new UnityEvent();

    /// <summary>
    /// Locks the lock
    /// </summary>
    /// <param name="requester">Object requesting the lock</param>
    public bool Lock(object requester)
    {
      bool wasUnlocked = IsUnlocked;
    
      if ( set.Add(requester) )
      {
        if ( wasUnlocked && IsLocked )
        {
          onLocked.Invoke();
        }
        return true;
      }
      return false;
    }

    /// <summary>
    /// Unlocks the lock
    /// </summary>
    /// <param name="requester">The object that originally requested the lock</param>
    public bool Unlock(object requester)
    {
      if ( set.Remove(requester) )
      {
        if ( IsUnlocked )
        {
          onUnlocked.Invoke();
        }
        return true;
      }
      return false;
    }

    public bool IsUnlocked
    {
      get { return set.Count == 0; }
    }

    public bool IsLocked
    {
      get { return set.Count != 0; }
    }

  }

}
