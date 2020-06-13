using UnityEngine;

namespace Gameframe.GUI
{
  /// <summary>
  /// Supersample is used to render the screen at a higher resolution and then scale it down to try and improve visual quality
  /// </summary>
  [ExecuteInEditMode]
  public class Supersample : MonoBehaviour
  {
    
    private RenderTexture _supersampleRenderTexture;
    
    [SerializeField]
    private UnityEngine.Camera cam;

    private const float Factor = 1.5f;

    private void Start()
    {
      _supersampleRenderTexture = new RenderTexture( Mathf.RoundToInt( Screen.width * Factor ), Mathf.RoundToInt( Screen.height * Factor ), 24, RenderTextureFormat.ARGB32 );
    }

    private void OnRenderImage( RenderTexture source, RenderTexture destination )
    {
      cam.targetTexture = _supersampleRenderTexture;
      cam.Render();
      cam.targetTexture = null;

      Graphics.Blit( _supersampleRenderTexture, destination );
    }

  }

}