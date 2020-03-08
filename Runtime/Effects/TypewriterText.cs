using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameframe.GUI
{

  public abstract class BaseTypewriterText : MonoBehaviour
  {
    [SerializeField]
    protected int charactersPerSecond = 10;

    public virtual int CharacterPerSecond
    {
      get => charactersPerSecond;
      set => charactersPerSecond = value;
    }
    
    [SerializeField]
    protected bool playOnEnable = false;

    protected Coroutine typeCoroutine = null;
    protected string currentMessage = string.Empty;

    public bool IsPlaying => typeCoroutine != null;

    [SerializeField]
    protected UnityEvent onComplete = new UnityEvent();
    public UnityEvent OnComplete => onComplete;
    
    private void OnEnable()
    {
      if ( playOnEnable )
      {
        Play();
      }
    }

    private void OnDisable()
    {
      if ( typeCoroutine != null )
      {
        Finish();
      }
    }

    public abstract void Play();
    
    public void Play(string message)
    {
      if ( typeCoroutine != null )
      {
        Finish();
      }

      currentMessage = message;
      typeCoroutine = StartCoroutine(RunType());
    }

    public virtual void Finish()
    {
      if (typeCoroutine != null)
      {
        StopCoroutine(typeCoroutine);
        typeCoroutine = null;
        onComplete.Invoke();
      }
    }

    protected abstract IEnumerator RunType();

    protected virtual void OnValidate()
    {
      if ( charactersPerSecond <= 0 )
      {
        charactersPerSecond = 1;
      }
    }

  }

  public class TypewriterText : BaseTypewriterText
  {
    [SerializeField]
    private Text text = null;

    [ContextMenu("Play")]
    public override void Play()
    {
      Play(text.text);
    }
    
    public override void Finish()
    {
      base.Finish();
      text.text = currentMessage;
    }

    protected override IEnumerator RunType()
    {
      string visibleMessage = string.Empty;
      var interval = 1.0f / charactersPerSecond;

      for (var i = 0; i < currentMessage.Length; i++)
      {
        visibleMessage = (visibleMessage + currentMessage[i]);
        text.text = visibleMessage;
        yield return new WaitForSeconds(interval);
      }

      typeCoroutine = null;
      onComplete.Invoke();
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      if (text == null)
      {
        text = GetComponent<Text>();
      }
    }

  }

}