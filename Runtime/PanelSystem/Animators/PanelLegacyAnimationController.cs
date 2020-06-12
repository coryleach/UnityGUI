using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
  public enum PanelNavigationDirection : int
  {
    Forward = 1,
    Reverse = -1
  };
  
  /// <summary>
  /// Uses legacy animations to animate panels forwards and reverse
  /// Add this to any panel view to play an animation on show or hide
  /// </summary>
  [RequireComponent(typeof(Animation)),DisallowMultipleComponent]
  public class PanelLegacyAnimationController : MonoBehaviour, IPanelAnimator
  {
    private Animation _animation = null;
    private Animation Animation
    {
      get
      {
        if (_animation == null)
        {
          _animation = GetComponent<Animation>();
        }
        return _animation;
      }
    }

    [Header("Show")]
    [SerializeField]
    private AnimationClip showAnimation = null;
    [SerializeField] private PanelNavigationDirection showPanelNavigationDirection = PanelNavigationDirection.Forward;

    [Header("Hide")]
    [SerializeField]
    private AnimationClip hideAnimation = null;
    [SerializeField] private PanelNavigationDirection hidePanelNavigationDirection = PanelNavigationDirection.Forward;

    private bool _animating = false;

    private Action callback = null;
    private float time = 0;
    private PanelNavigationDirection direction = PanelNavigationDirection.Forward;

    [SerializeField] 
    private bool debug = false;

    private void OnEnable()
    {
      if (debug) Debug.LogFormat("OnEnable {0}", this.name);
    }

    private void OnDisable()
    {
      if (debug) Debug.LogFormat("OnDisable {0}", this.name);
      FinishImmediate();
    }

    public async Task TransitionShowAsync()
    {
      FinishImmediate();
      bool finished = false;
      TransitionShow(() => { finished = true; });
      while (!finished)
      {
        await Task.Yield();
      }
    }
    
    public async Task TransitionHideAsync()
    {
      FinishImmediate();
      bool finished = false;
      TransitionHide(() => { finished = true; });
      while (!finished)
      {
        await Task.Yield();
      }
    }
    
    private void TransitionShow(Action finish)
    {
      if (showPanelNavigationDirection == PanelNavigationDirection.Forward)
      {
        StartAnimation(showAnimation, 0, showPanelNavigationDirection, finish);
      }
      else
      {
        StartAnimation(showAnimation, showAnimation.length, showPanelNavigationDirection, finish);
      }
    }

    private void TransitionHide(Action finish)
    {
      if (hidePanelNavigationDirection == PanelNavigationDirection.Forward)
      {
        StartAnimation(hideAnimation, 0, hidePanelNavigationDirection, finish);
      }
      else
      {
        StartAnimation(hideAnimation, hideAnimation.length, hidePanelNavigationDirection, finish);
      }
    }

    public void FinishImmediate()
    {
      if (!_animating)
      {
        return;
      }
      
      if (direction == PanelNavigationDirection.Forward)
      {
        time = Animation.clip.length;
      }
      else
      {
        time = 0;
      }
      
      Update();
    }

    private void Update()
    {
      if (_animating)
      {
        if (direction == PanelNavigationDirection.Forward)
        {
          time += Time.deltaTime;
          if (time > Animation.clip.length)
          {
            time = Animation.clip.length;
          }
        }
        else
        {
          time -= Time.deltaTime;
          if (time < 0)
          {
            time = 0;
          }
        }

        foreach (AnimationState state in Animation)
        {
          if (Animation.IsPlaying(state.name))
          {
            state.time = time;
          }
        }

        Animation.Sample();

        if (direction == PanelNavigationDirection.Forward)
        {
          if (time >= Animation.clip.length)
          {
            FinishAnimation();
          }
        }
        else
        {
          if (time <= 0)
          {
            FinishAnimation();
          }
        }

      }
      else
      {
        enabled = false;
      }
    }

    private void StartAnimation(AnimationClip clip, float startTime, PanelNavigationDirection panelNavigationDirection, Action onFinish)
    {
      if (_animating)
      {
        if (debug) Debug.Log("Starting While Playing");
        FinishAnimation();
      }

      if (debug) { Debug.Log("StartAnimation"); }

      if (clip == null)
      {
        //No animation exists so just do callback
        onFinish?.Invoke();
        return;
      }

      _animating = true;
      enabled = true;

      Animation.enabled = false;
      Animation.clip = clip;

      if (!Animation.IsPlaying(clip.name))
      {
        Animation.Play(clip.name);
      }

      time = startTime;
      direction = panelNavigationDirection;
      callback = onFinish;

      if (debug) Debug.Log("Callback Assigned");

      //Initialize
      foreach (AnimationState state in Animation)
      {
        if (Animation.IsPlaying(state.name))
        {
          state.time = time;
        }
      }

      Animation.Sample();

    }

    private void FinishAnimation()
    {
      if (debug) { Debug.Log("FinishAnimation"); }
      _animating = false;
      if (callback == null)
      {
        return;
      }
      
      //Problem can occur where callback will get nulled out if the callback happens to show the panel again
      //So we make sure to clear the callback member variable BEFORE calling the function
      var temp = callback;
      callback = null;
      temp.Invoke();

      if (debug)
      {
        Debug.Log("Callback Cleared And then Called");
      }
    }

  }

}
