using System.Collections;
using UnityEngine;

namespace Gameframe.GUI
{
    public class MaterialFloatAnimator : MonoBehaviour
    {
        [SerializeField]
        private Material material;
        
        [SerializeField, Range(0.0f, 10)] private float startValue;
        [SerializeField, Range(0.0f, 10)] private float endValue = 5.0f;
        [SerializeField, Range(0.1f, 50)] private float animationSpeed = 25;
    
        [SerializeField]
        private string propertyName = "_Size";

        private float currentValue;
        private Coroutine coroutine;

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

