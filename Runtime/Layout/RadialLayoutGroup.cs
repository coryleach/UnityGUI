using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gameframe.GUI.Utility;

namespace Gameframe.GUI.Layout
{
  public class RadialLayoutGroup : UIBehaviour, ILayoutGroup
  {

    [SerializeField] 
    private float radius;

    [Range(0f, 360f), SerializeField]
    private float shift;

    [Range(0f, 360f), SerializeField]
    private float field = 180f;

    [System.NonSerialized] private RectTransform _mRect;

    private RectTransform rectTransform
    {
      get
      {
        if (_mRect == null)
        {
          _mRect = GetComponent<RectTransform>();
        }
        return _mRect;
      }
    }

    [System.NonSerialized] private readonly List<RectTransform> _mRectChildren = new List<RectTransform>();
    protected List<RectTransform> rectChildren => _mRectChildren;

    private int DivisorOffset
    {
      get
      {
        if (field != 360)
        {
          return -1;
        }
        return 0;
      }
    }

    // ILayoutElement Interface
    protected virtual void CalculateLayoutInputHorizontal()
    {
      _mRectChildren.Clear();
      var toIgnoreList = ListPool<Component>.Get();
      for (int i = 0; i < rectTransform.childCount; i++)
      {
        var rect = rectTransform.GetChild(i) as RectTransform;
        if (rect == null || !rect.gameObject.activeInHierarchy)
          continue;

        rect.GetComponents(typeof(ILayoutIgnorer), toIgnoreList);

        if (toIgnoreList.Count == 0)
        {
          _mRectChildren.Add(rect);
          continue;
        }

        for (int j = 0; j < toIgnoreList.Count; j++)
        {
          var ignorer = (ILayoutIgnorer)toIgnoreList[j];
          if (!ignorer.ignoreLayout)
          {
            _mRectChildren.Add(rect);
            break;
          }
        }
      }
      ListPool<Component>.Release(toIgnoreList);
    }

    public void SetLayoutHorizontal()
    {
      CalculateLayoutInputHorizontal();

      if (_mRectChildren.Count == 0)
      {
        return;
      }

      float interval = 0;
      float offset = (shift - 90) * Mathf.Deg2Rad;

      if (_mRectChildren.Count > 1)
      {
        interval = Mathf.Deg2Rad * field / (_mRectChildren.Count + DivisorOffset);
      }

      for (int i = 0; i < _mRectChildren.Count; i++)
      {
        var child = _mRectChildren[i];
        var pt = child.anchoredPosition;
        pt.x = Mathf.Cos(interval * i + offset) * radius;
        child.anchoredPosition = pt;
      }
    }

    public void SetLayoutVertical()
    {
      if (_mRectChildren.Count == 0)
      {
        return;
      }

      float interval = 0;
      float offset = (shift - 90) * Mathf.Deg2Rad;

      if (_mRectChildren.Count > 1)
      {
        interval = Mathf.Deg2Rad * field / (_mRectChildren.Count + DivisorOffset);
      }

      for (int i = 0; i < _mRectChildren.Count; i++)
      {
        var child = _mRectChildren[i];
        var pt = child.anchoredPosition;
        pt.y = Mathf.Sin(interval * i + offset) * radius;
        child.anchoredPosition = pt;
      }
    }

    private void SetDirty()
    {
      if (!IsActive())
      {
        return;
      }

      if (!CanvasUpdateRegistry.IsRebuildingLayout())
      {
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
      }
      else
      {
        StartCoroutine(DelayedSetDirty(rectTransform));
      }
    }

    private static IEnumerator DelayedSetDirty(RectTransform rectTransform)
    {
      yield return null;
      LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
      SetDirty();
    }

#endif

  }

}