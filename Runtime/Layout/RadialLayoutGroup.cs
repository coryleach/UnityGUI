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
    float radius = 0;

    [Range(0f, 360f)]
    [SerializeField]
    float shift = 0;

    [Range(0f, 360f)]
    [SerializeField]
    float field = 180f;

    [System.NonSerialized] private RectTransform m_Rect;
    protected RectTransform rectTransform
    {
      get
      {
        if (m_Rect == null)
          m_Rect = GetComponent<RectTransform>();
        return m_Rect;
      }
    }

    [System.NonSerialized] private List<RectTransform> m_RectChildren = new List<RectTransform>();
    protected List<RectTransform> rectChildren { get { return m_RectChildren; } }

    protected int DivisorOffset
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
    public virtual void CalculateLayoutInputHorizontal()
    {
      m_RectChildren.Clear();
      var toIgnoreList = ListPool<Component>.Get();
      for (int i = 0; i < rectTransform.childCount; i++)
      {
        var rect = rectTransform.GetChild(i) as RectTransform;
        if (rect == null || !rect.gameObject.activeInHierarchy)
          continue;

        rect.GetComponents(typeof(ILayoutIgnorer), toIgnoreList);

        if (toIgnoreList.Count == 0)
        {
          m_RectChildren.Add(rect);
          continue;
        }

        for (int j = 0; j < toIgnoreList.Count; j++)
        {
          var ignorer = (ILayoutIgnorer)toIgnoreList[j];
          if (!ignorer.ignoreLayout)
          {
            m_RectChildren.Add(rect);
            break;
          }
        }
      }
      ListPool<Component>.Release(toIgnoreList);
      //m_Tracker.Clear();
    }

    public void SetLayoutHorizontal()
    {
      CalculateLayoutInputHorizontal();

      if (m_RectChildren.Count == 0)
      {
        return;
      }

      float interval = 0;
      float offset = (shift - 90) * Mathf.Deg2Rad;

      if (m_RectChildren.Count > 1)
      {
        interval = Mathf.Deg2Rad * field / (m_RectChildren.Count + DivisorOffset);
      }

      for (int i = 0; i < m_RectChildren.Count; i++)
      {
        var child = m_RectChildren[i];
        var pt = child.anchoredPosition;
        pt.x = Mathf.Cos(interval * i + offset) * radius;
        child.anchoredPosition = pt;
      }
    }

    public void SetLayoutVertical()
    {
      if (m_RectChildren.Count == 0)
      {
        return;
      }

      float interval = 0;
      float offset = (shift - 90) * Mathf.Deg2Rad;

      if (m_RectChildren.Count > 1)
      {
        interval = Mathf.Deg2Rad * field / (m_RectChildren.Count + DivisorOffset);
      }

      for (int i = 0; i < m_RectChildren.Count; i++)
      {
        var child = m_RectChildren[i];
        var pt = child.anchoredPosition;
        pt.y = Mathf.Sin(interval * i + offset) * radius;
        child.anchoredPosition = pt;
      }
    }

    protected void SetDirty()
    {
      if (!IsActive())
        return;

      if (!CanvasUpdateRegistry.IsRebuildingLayout())
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
      else
        StartCoroutine(DelayedSetDirty(rectTransform));
    }

    IEnumerator DelayedSetDirty(RectTransform rectTransform)
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