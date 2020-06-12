using TMPro;
using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(TextMeshEffectTMPro))]
    public class WavyText : MonoBehaviour, ITextMeshVertexEffect
    {
        [SerializeField] 
        private TextMeshEffectTMPro effectManager;

        [SerializeField] 
        protected Vector2 amplitude = Vector2.one;
        
        [SerializeField]
        protected float period = 0.15f;

        [SerializeField]
        protected float speed = 1f;

        [SerializeField] 
        protected bool resetTimeOnEnable = false;
        
        private float time;

        private void OnEnable()
        {
            if (resetTimeOnEnable)
            {
                time = 0;
            }
            effectManager.AddVertexEffect(this);
        }

        private void OnDisable()
        {
            effectManager.RemoveVertexEffect(this);
        }
        
        private void Update()
        {
            time += Time.deltaTime * speed;
        }

        public void UpdateVertexEffect(TMP_CharacterInfo charInfo, ref EffectData data)
        {
            var t = Mathf.PI * time;
            var offset = Mathf.PI * data.Index * period;
            Vector3 pt;
            pt.x = Mathf.Cos(t + offset) * amplitude.x;
            pt.y = Mathf.Sin(t + offset) * amplitude.y;
            pt.z = 0;
            data.LocalPosition = pt;
        }

        private void OnValidate()
        {
            if (effectManager == null)
            {
                effectManager = GetComponent<TextMeshEffectTMPro>();
            }
        }
        
    }

}

