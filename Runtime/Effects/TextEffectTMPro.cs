using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TextEffectTMPro : MonoBehaviour
{
    [SerializeField] 
    protected TextMeshProUGUI text;
    public Vector2 pivot = new Vector2(0.5f,0.5f);

    private Coroutine coroutine = null;
    private TMP_MeshInfo[] meshCache = null;

    private void Awake()
    {
        text.ForceMeshUpdate();
    }

    protected void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        Refresh();
        coroutine = StartCoroutine(Animate());
    }

    protected void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private void OnTextChanged(System.Object _text)
    {
        if (text == null)
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
            return;
        }
        
        if (!ReferenceEquals(_text, text))
        {
            return;
        }

        Refresh();
    }

    protected void Refresh()
    {
        meshCache = text.textInfo.CopyMeshInfoVertexData();
    }

    [Serializable]
    public struct CharacterTransform
    {
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale;
    }

    [SerializeField]
    private CharacterTransform[] characterTransform = null;
    
    private IEnumerator Animate()
    {
        while (true)
        {
            text.ForceMeshUpdate();
            
            var characterCount = text.textInfo.characterCount;

            if (characterTransform == null || characterCount > characterTransform.Length)
            {
                characterTransform = new CharacterTransform[text.textInfo.characterCount];
                for (var i = 0; i < characterTransform.Length; i++)
                {
                    characterTransform[i].localPosition = Vector3.zero;
                    characterTransform[i].localRotation = Vector3.zero;
                    characterTransform[i].localScale = Vector3.one;
                }
            }
            
            //For each character
            for (var i = 0; i < characterCount; i++)
            {
                var charInfo = text.textInfo.characterInfo[i];
                var materialIndex = charInfo.materialReferenceIndex;
                var vertexIndex = charInfo.vertexIndex;
                
                if (!charInfo.isVisible)
                {
                    continue;
                }

                var sourceVertices = meshCache[materialIndex].vertices;

                // Getting this from charInfo.vertex_TL, etc. yields the wrong values
                var sourceTopLeft = sourceVertices[vertexIndex + 1];
                var sourceTopRight = sourceVertices[vertexIndex + 2];
                var sourceBottomLeft = sourceVertices[vertexIndex + 0];
                var sourceBottomRight = sourceVertices[vertexIndex + 3];
                
                var offset = sourceBottomLeft + (sourceBottomRight - sourceBottomLeft) * pivot.x + (sourceTopLeft - sourceBottomLeft) * pivot.y;

                var anim = characterTransform[i];
                var matrix = Matrix4x4.TRS(anim.localPosition, Quaternion.Euler(anim.localRotation), anim.localScale);
                
                var destinationTopLeft = matrix.MultiplyPoint3x4(sourceTopLeft - offset) + offset;
                var destinationTopRight = matrix.MultiplyPoint3x4(sourceTopRight - offset) + offset;
                var destinationBottomLeft = matrix.MultiplyPoint3x4(sourceBottomLeft - offset) + offset;
                var destinationBottomRight = matrix.MultiplyPoint3x4(sourceBottomRight - offset) + offset;
                
                var destinationVertices = text.textInfo.meshInfo[materialIndex].vertices;
                destinationVertices[vertexIndex + 0] = destinationBottomLeft;
                destinationVertices[vertexIndex + 1] = destinationTopLeft;
                destinationVertices[vertexIndex + 2] = destinationTopRight;
                destinationVertices[vertexIndex + 3] = destinationBottomRight;
            }
            
            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            
            yield return null;
        }
    }

    protected void OnValidate()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        pivot.x = Mathf.Clamp01(pivot.x);
        pivot.y = Mathf.Clamp01(pivot.y);
    }
}
