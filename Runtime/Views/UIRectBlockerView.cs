using Gameframe.GUI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Gameframe.GUI
{
  public class UIRectBlockerView : MonoBehaviour
  {
    private RectTransform _blockerParent = null;
    
    private RectTransform _leftBlocker = null;
    private RectTransform _rightBlocker = null;
    private RectTransform _topBlocker = null;
    private RectTransform _bottomBlocker = null;

    [SerializeField]
    private RectTransform _passThroughRect = null;

    private Canvas _parentCanvas = null;
    
    private void Awake()
    {
      _blockerParent = new GameObject("blocker").GetOrAddComponent<RectTransform>();
      _blockerParent.transform.parent = transform;
      _blockerParent.gameObject.SetActive(_passThroughRect != null);
      _blockerParent.anchorMin = Vector2.zero;
      _blockerParent.anchorMax = Vector2.one;
      _blockerParent.sizeDelta = Vector2.zero;
      _blockerParent.anchoredPosition = Vector2.zero;
      
      _leftBlocker = new GameObject("leftBlocker").GetOrAddComponent<RectTransform>();
      _rightBlocker = new GameObject("rightBlocker").GetOrAddComponent<RectTransform>();
      _topBlocker = new GameObject("topBlocker").GetOrAddComponent<RectTransform>();
      _bottomBlocker = new GameObject("bottomBlocker").GetOrAddComponent<RectTransform>();

      _leftBlocker.parent = _blockerParent;
      _rightBlocker.parent = _blockerParent;
      _topBlocker.parent = _blockerParent;
      _bottomBlocker.parent = _blockerParent;
      
      _leftBlocker.pivot = new Vector2(0, 0.5f);
      _bottomBlocker.pivot = new Vector2(0.5f, 0f);
      _topBlocker.pivot = new Vector2(0.5f, 1f);
      _rightBlocker.pivot = new Vector2(1, 0.5f);
      
      _leftBlocker.anchoredPosition = Vector2.zero;
      _rightBlocker.anchoredPosition = Vector2.zero;
      _topBlocker.anchoredPosition = Vector2.zero;
      _bottomBlocker.anchoredPosition = Vector2.zero;
      
      _parentCanvas = GetComponentInParent<Canvas>();

      var img = _leftBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;
      
      img = _rightBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;
      
      img = _topBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;
      
      img = _bottomBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;
      
      Refresh();
    }

    public void Show(RectTransform passThrough)
    {
      _blockerParent.gameObject.SetActive(true);
      _passThroughRect = passThrough;
      Refresh();
    }

    [ContextMenu("Show")]
    public void Show()
    {
      _blockerParent.gameObject.SetActive(true);
      Refresh();
    }

    [ContextMenu("Dismiss")]
    public void Dismiss()
    {
      _blockerParent.gameObject.SetActive(false);
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
      Vector2 topLeft = Vector2.zero;
      Vector2 bottomRight = Vector2.zero;

      if (_passThroughRect != null)
      {
        Vector3[] corners = new Vector3[4];
        _passThroughRect.GetWorldCorners( corners );
        
        //Check for a world space camera if we have one
        var worldCamera = _parentCanvas.worldCamera;
        
        //OK for camera to be null here if canvas render mode is screen space overlay
        Vector3 topLeftScreenPoint = RectTransformUtility.WorldToScreenPoint(worldCamera, corners[0]);
        Vector3 bottomRightScreenPoint = RectTransformUtility.WorldToScreenPoint(worldCamera, corners[2]);

        RectTransformUtility.WorldToScreenPoint(worldCamera, corners[0]);
        
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_blockerParent, topLeftScreenPoint, worldCamera,
          out topLeft))
        {
          Debug.LogError("plane of the RectTransform was not hit");
          return;
        }
    
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_blockerParent, bottomRightScreenPoint, worldCamera,
          out bottomRight))
        {
          Debug.LogError("plane of the RectTransform was not hit");
          return;
        }
      }

      
      
      _leftBlocker.anchorMin = Vector2.zero;
      _leftBlocker.anchorMax = new Vector2(0.5f,1f);
      _leftBlocker.sizeDelta = new Vector2( topLeft.x, 0 );

      _bottomBlocker.anchorMin = new Vector2(0f,0f);
      _bottomBlocker.anchorMax = new Vector2(1f,0.5f);
      _bottomBlocker.sizeDelta = new Vector2( 0, topLeft.y );

      _rightBlocker.anchorMin = new Vector2(0.5f,0f);
      _rightBlocker.anchorMax = new Vector2(1f,1f);
      _rightBlocker.sizeDelta = new Vector2( -bottomRight.x, 0 );

      _topBlocker.anchorMin = new Vector2(0f,0.5f);
      _topBlocker.anchorMax = new Vector2(1f,1f);
      _topBlocker.sizeDelta = new Vector2( 0, -bottomRight.y );
    }
    
  }
}