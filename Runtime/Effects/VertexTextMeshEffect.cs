using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameframe.GUI
{
  /// <summary>
  /// Base class for text mesh effects that modify just vertices
  /// </summary>
  [RequireComponent(typeof(TextMeshEffectTMPro))]
  public abstract class VertexTextMeshEffect : PlayableTextMeshEffect, ITextMeshVertexEffect
  {

    protected override void AddToManager(TextMeshEffectTMPro effectManager)
    {
      effectManager.AddVertexEffect(this);
    }

    protected override void RemoveFromManager(TextMeshEffectTMPro effectManager)
    {
      effectManager.RemoveVertexEffect(this);
    }

    public abstract void UpdateVertexEffect(TMP_CharacterInfo charInfo, ref EffectData data);
  }
}
