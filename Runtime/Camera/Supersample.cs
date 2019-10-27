using UnityEngine;

namespace Gameframe.GUI
{
  /// <summary>
  /// Supersample is used to render the screen at a higher resolution and then scale it down to try and improve visual quality
  /// </summary>
  [ExecuteInEditMode]
  public class Supersample : MonoBehaviour
  {
    
    RenderTexture supersampleRenderTexture;
    public UnityEngine.Camera cam;

    const float factor = 1.5f;

    void Start()
    {
      supersampleRenderTexture = new RenderTexture( Mathf.RoundToInt( Screen.width * factor ), Mathf.RoundToInt( Screen.height * factor ), 24, RenderTextureFormat.ARGB32 );
    }

    void OnRenderImage( RenderTexture source, RenderTexture destination )
    {
      cam.targetTexture = supersampleRenderTexture;
      cam.Render();
      cam.targetTexture = null;

      Graphics.Blit( supersampleRenderTexture, destination );
    }

  }

}