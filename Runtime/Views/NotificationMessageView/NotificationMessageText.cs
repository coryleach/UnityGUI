using System.Collections;
using UnityEngine;
using TMPro;
using Gameframe.GUI.Pooling;
using Gameframe.GUI.Tween;

namespace Gameframe.GUI
{
  public class NotificationMessageText : PoolableGameObject
  {
    [SerializeField]
    private TextMeshProUGUI text = null;
  
    [SerializeField]
    private float moveDuration = 0.5f;
  
    [SerializeField]
    private float stayDuration = 1.5f;
  
    [SerializeField]
    private float fadeDelay = 0.75f;
  
    [SerializeField]
    private float fadeDuration = 0.5f;
  
    private int bumps = 0;
    
    public RectTransform rectTransform => transform as RectTransform;
  
    public void Message(string message)
    {
      Message(message, Color.white);
    }
    
    public void Message(string message, Color color)
    {
      text.text = message;
      rectTransform.anchoredPosition = new Vector2(0, -rectTransform.sizeDelta.y);
      rectTransform.DoAnchorPosY(0, moveDuration);//.SetEase(Ease.OutSine);
      text.color = new Color(1,1,1,0);
      text.DoColor(color, moveDuration);
      StartCoroutine(Fade());
    }
  
    public void BumpUp()
    {
      bumps += 1;
      rectTransform.DoAnchorPosY(bumps * rectTransform.sizeDelta.y, moveDuration);//.SetEase(Ease.OutSine);
    }
  
    private IEnumerator Fade()
    {
      float time = 0;
      while ( time < stayDuration && bumps <= 0 )
      {
        time += Time.deltaTime;
        yield return null;
      }
      yield return new WaitForSeconds(fadeDelay);
      text.DoColor(Color.clear, fadeDuration);
      BumpUp();
      yield return new WaitForSeconds(fadeDuration);
      Despawn();
    }
  
    private NotificationMessageView messageView = null;
    
    public override void OnPoolableSpawned()
    {
      base.OnPoolableSpawned();
      messageView = GetComponentInParent<NotificationMessageView>();
    }
  
    public override void OnPoolableDespawn()
    {
      base.OnPoolableDespawn();
      bumps = 0;
      rectTransform.DoKillTweens();
      rectTransform.anchoredPosition = new Vector2(0, -80);
      if (messageView != null)
      {
        messageView.RemoveText(this);
      }
    }
  
  }
}

