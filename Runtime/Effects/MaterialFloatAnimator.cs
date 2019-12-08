using System.Collections;
using UnityEngine;

namespace Gameframe.GUI
{
    public class MaterialFloatAnimator : MonoBehaviour
    {
        public Material material;

        [Range(0.0f, 10)] public float startValue = 0f;
        [Range(0.0f, 10)] public float endValue = 5.0f;
        [Range(0.1f, 50)] public float animationSpeed = 25;
    
        public string propertyName = "_Size";

        private float currentValue;

        private Coroutine coroutine = null;

        private void Start()
        {
            material.SetFloat(propertyName, startValue);
        }
        
        private IEnumerator _Play(float _startValue, float _endValue)
        {
            currentValue = material.GetFloat(propertyName);

            int direction = 1;
            if (_endValue < _startValue)
            {
                direction = -1;
            }
        
            while (Check(currentValue,_endValue,direction))
            {
                currentValue += Time.deltaTime * animationSpeed * direction;

                if (direction == 1 && currentValue >= _endValue)
                {
                    currentValue = _endValue;
                }
                else if ( direction == -1 && currentValue <= _endValue )
                {
                    currentValue = _endValue;
                }

                material.SetFloat(propertyName, currentValue);
                yield return null;
            }

            coroutine = null;
        }

        private static bool Check(float currentValue, float targetValue, int direction)
        {
            if (direction == 1)
            {
                return currentValue < targetValue;
            }
            return currentValue > targetValue;
        }
    
        [ContextMenu("PlayForward")]
        public void PlayForward()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(_Play(startValue,endValue));
        }

        [ContextMenu("PlayBackward")]
        public void PlayBackward()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(_Play(endValue,startValue));   
        }
    
    }
}

