using UnityEngine;
using UnityEngine.UI;

namespace Gameframe.GUI.Layout
{

  public abstract class HorizontalOrVerticalPagedLayoutGroup : LayoutGroup, ILayoutSelfController
  {

    public float spacing = 0;

    [SerializeField]
    protected RectTransform viewport;
    public RectTransform Viewport
    {
      get { return viewport; }
    }

  }

}