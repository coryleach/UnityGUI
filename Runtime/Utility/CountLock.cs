using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.Utility
{

  //Similar in function to a mutex. 
  //General use case is blocking the UI while multiple actions are going on.
  // For example UI could be blocked while a request and a transition is happening.
  // Both actions "lock" the UI by calling Lock.Close and the Lock doesn't open again till both have finished and called Lock.Open
  [System.Serializable]
  public class CountLock
  {

    int count = 0;

    public UnityEvent onUnlocked = new UnityEvent();
    public UnityEvent onLocked = new UnityEvent();

    public void Lock()
    {
      if ( count == 0 )
      {
        onLocked.Invoke();
      }
      count += 1;
    }

    public void Unlock()
    {
      if ( count > 0 )
      {
        if ( count == 1 )
        {
          onUnlocked.Invoke();
        }
        count -= 1;
      }
      else
      {
        //Throw Exception?
        Debug.LogError( "Unlock called too many times!" );
      }
    }

    public bool IsUnlocked
    {
      get { return count == 0; }
    }

    public bool IsLocked
    {
      get { return count != 0; }
    }

  }

}
