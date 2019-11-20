using System.Collections.Generic;
using Gameframe.GUI.Pooling;
using UnityEngine;

namespace Gameframe.GUI
{
  public class NotificationMessageView : MonoBehaviour
  {
    
    [SerializeField]
    private NotificationMessageText prefab = null;
  
    private Pool pool = null;
    private readonly List<NotificationMessageText> activeTexts = new List<NotificationMessageText>();
  
    private void Start()
    {
      prefab.gameObject.SetActive(false);
      pool = new Pool(prefab);
    }
  
    private void OnDisable()
    {
      //cleanup any active texts
      //iterating backwards because Despawn will remove from list
      for (var i = activeTexts.Count - 1; i >= 0; i--)
      {
        activeTexts[i].Despawn();
      }
    }
  
    [ContextMenu("Test Spawn")]
    public void Spawn()
    {
      AddMessage("A Ghost Smiles at you.");
    }
  
    public void AddMessage(string message)
    {
      AddMessage(message,Color.white);
    }
  
    public void AddMessage(string message, Color color)
    {
      if (pool == null)
      {
        return;
      }
      
      var text = pool.Spawn(transform) as NotificationMessageText;
      text.gameObject.SetActive(true);
      text.Message(message,color);
  
      foreach (var activeText in activeTexts)
      {
        activeText.BumpUp();
      }
  
      activeTexts.Add(text);
    }
  
    public void RemoveText(NotificationMessageText battleMessageText)
    {
      activeTexts.Remove(battleMessageText);
    }
  
  }
}

  
