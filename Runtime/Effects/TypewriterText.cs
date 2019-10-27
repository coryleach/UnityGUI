using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameframe.GUI
{

  public class TypewriterText : MonoBehaviour
  {

    [SerializeField]
    private Text text = null;

    [SerializeField]
    private int charactersPerSecond = 10;

    [SerializeField]
    private bool playOnEnable = false;

    private Coroutine _typeCoroutine = null;
    private string _currentMessage = string.Empty;

    public bool IsPlaying => _typeCoroutine != null;

    private void OnEnable()
    {
      if ( playOnEnable )
      {
        Play();
      }
    }

    private void OnDisable()
    {
      if ( _typeCoroutine != null )
      {
        Finish();
      }
    }

    [ContextMenu("Play")]
    public void Play()
    {
      Play(text.text);
    }

    public void Play(string message)
    {
      if ( _typeCoroutine != null )
      {
        Finish();
      }

      _currentMessage = message;
      _typeCoroutine = StartCoroutine(RunType());
    }

    public void Finish()
    {
      if (_typeCoroutine != null)
      {
        StopCoroutine(_typeCoroutine);
        _typeCoroutine = null;
      }
      text.text = _currentMessage;
    }

    private IEnumerator RunType()
    {
      string visibleMessage = string.Empty;
      float interval = 1.0f / charactersPerSecond;

      for (int i = 0; i < _currentMessage.Length; i++)
      {
        visibleMessage = (visibleMessage + _currentMessage[i]);
        text.text = visibleMessage;
        yield return new WaitForSeconds(interval);
      }

      _typeCoroutine = null;
    }

    private void OnValidate()
    {
      if ( charactersPerSecond <= 0 )
      {
        charactersPerSecond = 1;
      }
    }

  }

}