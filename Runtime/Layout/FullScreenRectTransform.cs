using System.Collections.Generic;
using UnityEngine;
using Gameframe.GUI.Extensions;
using UnityEngine.UI;

namespace Gameframe.GUI.Layout
{

  /// <summary>
  /// This component will size any rect transform to fill the screen no matter the sizing, or positioning of its parents
  /// </summary>
  public class FullScreenRectTransform : LayoutGroup
  {
    private readonly float[] offsets = new float[4];

    private static void SumOffsets(IList<float> offsets, RectTransform currentRectTransform)
    {
      if (currentRectTransform == null)
      {
        return;
      }

      var parent = currentRectTransform.parent as RectTransform;
      if (parent == null)
      {
        return;
      }

      offsets[(int)RectTransform.Edge.Left] += currentRectTransform.GetInsetFromParentLeftEdge(parent);
      offsets[(int)RectTransform.Edge.Right] += currentRectTransform.GetInsetFromParentRightEdge(parent);
      offsets[(int)RectTransform.Edge.Top] += currentRectTransform.GetInsetFromParentTopEdge(parent);
      offsets[(int)RectTransform.Edge.Bottom] += currentRectTransform.GetInsetFromParentBottomEdge(parent);

      SumOffsets(offsets, parent);
    }

    public override void CalculateLayoutInputHorizontal()
    {
      //Fit Rect to parent
      var myRectTransform = rectTransform;
      myRectTransform.anchorMin = Vector2.zero;
      myRectTransform.anchorMax = Vector2.one;
      myRectTransform.anchoredPosition = Vector2.zero;
      myRectTransform.sizeDelta = Vector2.zero;

      //Add Padding
      offsets[(int)RectTransform.Edge.Left] = -padding.left;
      offsets[(int)RectTransform.Edge.Right] = -padding.right;
      offsets[(int)RectTransform.Edge.Top] = -padding.top;
      offsets[(int)RectTransform.Edge.Bottom] = -padding.bottom;

      //Calculate parent offsets from all edges
      var parent = rectTransform.parent as RectTransform;
      SumOffsets(offsets, parent);

      m_Tracker.Clear();
    }

    public override void CalculateLayoutInputVertical()
    {
      //Required implementation of abstract method but we don't need to do anything here
    }

    public override void SetLayoutHorizontal()
    {
      //Apply horizontal positions
      float width = rectTransform.rect.width + offsets[(int)RectTransform.Edge.Left] + offsets[(int)RectTransform.Edge.Right];
      rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -offsets[(int)RectTransform.Edge.Left], width);
    }

    public override void SetLayoutVertical()
    {
      //Apply vertical positions
      float height = rectTransform.rect.height + offsets[(int)RectTransform.Edge.Top] + offsets[(int)RectTransform.Edge.Bottom];
      rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -offsets[(int)RectTransform.Edge.Top], height);
      m_Tracker.Add(this, rectTransform, DrivenTransformProperties.All);
    }

  }

}