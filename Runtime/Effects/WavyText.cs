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
        private float radius = 2;

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
            var offset = Mathf.PI * charInfo.index * period;
            Vector3 pt;
            pt.x = Mathf.Cos(t + offset) * radius;
            pt.y = Mathf.Sin(t + offset) * radius;
            pt.z = 0;
            data.localPosition = pt;
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

