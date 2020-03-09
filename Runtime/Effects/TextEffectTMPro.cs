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
        time = 0;
        InitializeCharacterTransforms();
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
        public Quaternion localRotation;
        public Vector3 localScale;
        public byte alpha;
    }

    [SerializeField]
    protected CharacterTransform[] characterTransforms = null;

    [SerializeField] 
    private float radius = 2;

    [SerializeField]
    protected float period = 0.15f;

    [SerializeField] 
    protected float speed = 1f;

    [SerializeField] protected Vector2 move = Vector2.zero;
    
    [SerializeField] protected  Vector2 startScale = Vector2.one;
    
    private float time = 0;
    
    private void Update()
    {
        time += Time.deltaTime * speed;
        var t = Mathf.PI * time;
        for (var i = 0; i < characterTransforms.Length; i++)
        {
            var offset = Mathf.PI * i * period;
            Vector3 pt;
            pt.x = Mathf.Cos(t + offset) * radius;
            pt.y = Mathf.Sin(t + offset) * radius;
            pt.z = 0;
            characterTransforms[i].localPosition = pt;

            float charsPerSecond = 1.0f / 15f;
            float charactersShowing = time / charsPerSecond;
            float progress = charactersShowing - Mathf.Floor(charactersShowing);

            if (Mathf.FloorToInt(charactersShowing) > i)
            {
                //characterTransforms[i].alpha = 255;
                characterTransforms[i].localScale = Vector3.one;
            }
            else if ( Mathf.FloorToInt(charactersShowing) == i )
            {
                //characterTransforms[i].alpha = (byte)Mathf.FloorToInt(progress * 255);
                characterTransforms[i].localScale = Vector3.Lerp(startScale, Vector3.one, progress);
                characterTransforms[i].localPosition += Vector3.Lerp( move, Vector3.zero, progress);
            }
            else
            {
                //characterTransforms[i].alpha = 0;
            }
            
            characterTransforms[i].alpha = (byte)Mathf.FloorToInt(Mathf.Clamp01(charactersShowing - i) * 255);

        }
    }

    protected void UpdatePosition()
    {
        
    }
    
    protected void UpdateAlpha()
    {
        
    }

    protected void InitializeCharacterTransforms(int startIndex = 0)
    {
        for (var i = startIndex; i < characterTransforms.Length; i++)
        {
            characterTransforms[i].localPosition = Vector3.zero;
            characterTransforms[i].localRotation = Quaternion.identity;
            characterTransforms[i].localScale = Vector3.one;
            characterTransforms[i].alpha = 0;
        }
    }
    
    private IEnumerator Animate()
    {
        while (true)
        {
            text.ForceMeshUpdate();
            
            var characterCount = text.textInfo.characterCount;

            if (characterTransforms == null || characterCount > characterTransforms.Length)
            {
                var old = characterTransforms;
                var startIndex = 0 + (characterTransforms?.Length ?? 0);
                characterTransforms = new CharacterTransform[text.textInfo.characterCount];
                old?.CopyTo(characterTransforms,0);
                InitializeCharacterTransforms(startIndex);
            }
            
            //For each character
            for (var i = 0; i < characterCount; i++)
            {
                var charInfo = text.textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                {
                    continue;
                }
                
                var materialIndex = charInfo.materialReferenceIndex;
                var vertexIndex = charInfo.vertexIndex;
                
                //Do Vertices
                var sourceVertices = meshCache[materialIndex].vertices;

                // Getting this from charInfo.vertex_TL, etc. yields the wrong values
                var sourceBottomLeft = sourceVertices[vertexIndex + 0];
                var sourceTopLeft = sourceVertices[vertexIndex + 1];
                var sourceTopRight = sourceVertices[vertexIndex + 2];
                var sourceBottomRight = sourceVertices[vertexIndex + 3];
                
                var offset = sourceBottomLeft + (sourceBottomRight - sourceBottomLeft) * pivot.x + (sourceTopLeft - sourceBottomLeft) * pivot.y;

                var anim = characterTransforms[i];
                var matrix = Matrix4x4.TRS(anim.localPosition, anim.localRotation, anim.localScale);
                
                var destinationTopLeft = matrix.MultiplyPoint3x4(sourceTopLeft - offset) + offset;
                var destinationTopRight = matrix.MultiplyPoint3x4(sourceTopRight - offset) + offset;
                var destinationBottomLeft = matrix.MultiplyPoint3x4(sourceBottomLeft - offset) + offset;
                var destinationBottomRight = matrix.MultiplyPoint3x4(sourceBottomRight - offset) + offset;
                
                var destinationVertices = text.textInfo.meshInfo[materialIndex].vertices;
                destinationVertices[vertexIndex + 0] = destinationBottomLeft;
                destinationVertices[vertexIndex + 1] = destinationTopLeft;
                destinationVertices[vertexIndex + 2] = destinationTopRight;
                destinationVertices[vertexIndex + 3] = destinationBottomRight;
                
                //Do Colors
                var sourceColors = meshCache[materialIndex].colors32;
                var colorBottomLeft = sourceColors[vertexIndex + 0];
                var colorTopLeft = sourceColors[vertexIndex + 1];
                var colorTopRight = sourceColors[vertexIndex + 2];
                var colorBottomRight = sourceColors[vertexIndex + 3];

                colorTopLeft.a = anim.alpha;
                colorTopRight.a = anim.alpha;
                colorBottomLeft.a = anim.alpha;
                colorBottomRight.a = anim.alpha;
                
                var destinationColors = text.textInfo.meshInfo[materialIndex].colors32;
                destinationColors[vertexIndex + 0] = colorBottomLeft;
                destinationColors[vertexIndex + 1] = colorTopLeft;
                destinationColors[vertexIndex + 2] = colorTopRight;
                destinationColors[vertexIndex + 3] = colorBottomRight;
            }
            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32);
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
