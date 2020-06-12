using System;
using UnityEngine;

namespace Gameframe.GUI.Extensions
{

  public static class RectTransformExt
  {

    public static float GetInsetFromParentTopEdge(this RectTransform child, RectTransform parent)
    {
      float parentPivotYDistToParentTop = (1f - parent.pivot.y) * parent.rect.height;
      float childLocPosY = child.localPosition.y;

      return parentPivotYDistToParentTop - child.rect.yMax - childLocPosY;
    }

    public static float GetInsetFromParentBottomEdge(this RectTransform child, RectTransform parent)
    {
      float parentPivotYDistToParentBottom = parent.pivot.y * parent.rect.height;
      float childLocPosY = child.localPosition.y;

      return parentPivotYDistToParentBottom + child.rect.yMin + childLocPosY;
    }

    public static float GetInsetFromParentLeftEdge(this RectTransform child, RectTransform parent)
    {
      float parentPivotXDistToParentLeft = parent.pivot.x * parent.rect.width;
      float childLocPosX = child.localPosition.x;

      return parentPivotXDistToParentLeft + child.rect.xMin + childLocPosX;
    }

    public static float GetInsetFromParentRightEdge(this RectTransform child, RectTransform parent)
    {
      float parentPivotXDistToParentRight = (1f - parent.pivot.x) * parent.rect.width;
      float childLocPosX = child.localPosition.x;

      return parentPivotXDistToParentRight - child.rect.xMax - childLocPosX;
    }

    public static float GetInsetFromParentEdge(this RectTransform child, RectTransform parent, RectTransform.Edge parentEdge)
    {
      switch (parentEdge)
      {
        case RectTransform.Edge.Top:
          return child.GetInsetFromParentTopEdge(parent);
        case RectTransform.Edge.Bottom:
          return child.GetInsetFromParentBottomEdge(parent);
        case RectTransform.Edge.Left:
          return child.GetInsetFromParentLeftEdge(parent);
        case RectTransform.Edge.Right:
          return child.GetInsetFromParentRightEdge(parent);
      }
      return 0f;
    }

    /// <summary>
    /// Optimized version of SetInsetAndSizeFromParentEdgeWithCurrentAnchors(RectTransform.Edge fixedEdge, float newInset, float newSize) when parent is known
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="fixedEdge"></param>
    /// <param name="newInset"></param>
    /// <param name="newSize"></param>
    public static void SetInsetAndSizeFromParentEdgeWithCurrentAnchors(this RectTransform child, RectTransform parent, RectTransform.Edge fixedEdge, float newInset, float newSize)
    {
      Vector2 offsetMin = child.offsetMin;
      Vector2 offsetMax = child.offsetMax;

      float currentOffset, offsetChange;

      switch (fixedEdge)
      {
        case RectTransform.Edge.Bottom:
          currentOffset = child.GetInsetFromParentBottomEdge(parent);
          offsetChange = newInset - currentOffset;
          offsetMax.y += (newSize - child.rect.height) + offsetChange;
          offsetMin.y += offsetChange;
          break;

        case RectTransform.Edge.Top:
          currentOffset = child.GetInsetFromParentTopEdge(parent);
          offsetChange = newInset - currentOffset;
          offsetMin.y -= (newSize - child.rect.height) + offsetChange;
          offsetMax.y -= offsetChange;
          break;

        case RectTransform.Edge.Left:
          currentOffset = child.GetInsetFromParentLeftEdge(parent);
          offsetChange = newInset - currentOffset;
          offsetMax.x += (newSize - child.rect.width) + offsetChange;
          offsetMin.x += offsetChange;
          break;

        case RectTransform.Edge.Right:
          currentOffset = child.GetInsetFromParentRightEdge(parent);
          offsetChange = newInset - currentOffset;
          offsetMin.x -= (newSize - child.rect.width) + offsetChange;
          offsetMax.x -= offsetChange;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(fixedEdge));
      }

      child.offsetMin = offsetMin;
      child.offsetMax = offsetMax;
    }

  }

}
