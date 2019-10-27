using UnityEngine;

namespace Gameframe.GUI.Layout
{

  public class VerticalPagedLayoutGroup : HorizontalOrVerticalPagedLayoutGroup
  {

    public override void CalculateLayoutInputHorizontal()
    {
      base.CalculateLayoutInputHorizontal();
      if (Viewport != null)
      {
        float viewportSize = Viewport.rect.width;
        float totalSize = viewportSize + padding.left + padding.right;
        SetLayoutInputForAxis(totalSize, totalSize, totalSize, 0);
      }
    }

    public override void CalculateLayoutInputVertical()
    {
      if (Viewport != null)
      {
        float viewportSize = Viewport.rect.height;
        float totalSize = viewportSize * rectChildren.Count + padding.top + padding.bottom;
        SetLayoutInputForAxis(totalSize, totalSize, totalSize, 1);
      }
    }

    public override void SetLayoutHorizontal()
    {
      if (Viewport == null)
      {
        return;
      }

      float viewportSize = Viewport.rect.width;

      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, viewportSize);
      m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDelta);

      viewportSize -= padding.left + padding.right;

      for (int i = 0; i < rectChildren.Count; i++)
      {
        var child = rectChildren[i];
        child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, padding.left, viewportSize);
        m_Tracker.Add(this, child, DrivenTransformProperties.SizeDelta | DrivenTransformProperties.AnchoredPosition);
      }
    }

    public override void SetLayoutVertical()
    {
      if (Viewport == null)
      {
        return;
      }

      float viewportSize = Viewport.rect.height;
      float totalSize = viewportSize * rectChildren.Count + padding.top + padding.bottom;

      if ( rectChildren.Count > 0 )
      {
        totalSize += spacing * (rectChildren.Count - 1);
      }

      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalSize);

      for (int i = 0; i < rectChildren.Count; i++)
      {
        var child = rectChildren[i];
        child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, padding.top + (i * (viewportSize + spacing)), viewportSize);
      }
    }

  }

}
