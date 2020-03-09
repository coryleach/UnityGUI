/*
 * Copyright (c) 2020 Cory R. Leach
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using UnityEngine;

namespace Gameframe.GUI.Tween
{
    public enum Easing
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InSine,
        OutSine,
        InOutSine,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic, 
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce,
        Spring
    }

    /// <summary>
    /// Static class containing various ease methods
    /// </summary>
    public static class EaseFunctions
    {
        private readonly static Func<float, float, float> Pow = Mathf.Pow;
        private readonly static Func<float, float> Sin = Mathf.Sin;
        private readonly static Func<float, float> Cos = Mathf.Cos;
        private readonly static Func<float, float> Sqrt = Mathf.Sqrt;
        private readonly static float PI = Mathf.PI;
        
        private readonly static float c1 = 1.70158f;
        private readonly static float c2 = c1 * 1.525f;
        private readonly static float c3 = c1 + 1;
        private readonly static float c4 = (2 * PI) / 3;
        private readonly static float c5 = (2 * PI) / 4.5f;

        public static float Ease(Easing easeType, float x)
        {
            return Get(easeType).Invoke(x);
        }
        
        public static Func<float,float> Get(Easing easeType)
        {
            switch (easeType)
            {
                case Easing.Linear:
                    return Linear;
                case Easing.InQuad:
                    return InQuad;
                case Easing.OutQuad:
                    return OutQuad;
                case Easing.InOutQuad:
                    return InOutQuad;
                case Easing.InCubic:
                    return InCubic;
                case Easing.OutCubic:
                    return OutCubic;
                case Easing.InOutCubic:
                    return InOutCubic;
                case Easing.InSine:
                    return InSine;
                case Easing.OutSine:
                    return OutSine;
                case Easing.InOutSine:
                    return InOutSine;
                case Easing.InExpo:
                    return InExpo;
                case Easing.OutExpo:
                    return OutExpo;
                case Easing.InOutExpo:
                    return InOutExpo;
                case Easing.InCirc:
                    return InCirc;
                case Easing.OutCirc:
                    return OutCirc;
                case Easing.InOutCirc:
                    return InOutCirc;
                case Easing.InBack:
                    return InBack;
                case Easing.OutBack:
                    return OutBack;
                case Easing.InOutBack:
                    return InOutBack;
                case Easing.InElastic:
                    return InElastic;
                case Easing.OutElastic:
                    return OutElastic;
                case Easing.InOutElastic:
                    return InOutElastic;
                case Easing.InBounce:
                    return InBounce;
                case Easing.OutBounce:
                    return OutBounce;
                case Easing.InOutBounce:
                    return InOutBounce;
                case Easing.Spring:
                    return Spring;
                default:
                    return Linear;
            }
        }
        
        public static float InQuad(float x)
        {
            return x * x;
        }

        public static float OutQuad(float x)
        {
            return 1 - (1 - x) * (1 - x);
        }

        public static float InOutQuad(float x)
        {
            return x < 0.5f ? (2 * x * x) : 1 - Pow(-2 * x + 2, 2) * 0.5f;
        }

        public static float InCubic(float x)
        {
            return x * x * x;
        }

        public static float OutCubic(float x)
        {
            return 1 - Pow(1 - x, 3);
        }

        public static float InOutCubic(float x)
        {
            return x < 0.5f ? 4 * x * x * x : 1 - Pow(-2 * x + 2, 3) * 0.5f;
        }

        public static float InSine(float x)
        {
            return 1 - Cos((x * PI) * 0.5f);
        }

        public static float OutSine(float x)
        {
            return Sin((x * PI) * 0.5f);
        }

        public static float InOutSine(float x)
        {
            return -(Cos(PI * x)) * 0.5f;
        }

        public static float InExpo(float x)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return x == 0 ? 0 : Pow(2, 10 * x - 10);
        }

        public static float OutExpo(float x)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return x == 1 ? 1 : 1 - Pow(2, -10 * x);
        }

        public static float InOutExpo(float x)
        {
            switch (x)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return x < 0.5f ? Pow(2, 20 * x - 10) * 0.5f : (2 - Pow(2, -20 * x + 10)) * 0.5f;
            }
        }

        public static float InCirc(float x)
        {
            return 1 - Sqrt(1 - Pow(x, 2));
        }
        
        public static float OutCirc(float x)
        {
            return Sqrt(1 - Pow(x - 1, 2));
        }

        public static float InOutCirc(float x)
        {
            return x < 0.5f ? (1 - Sqrt(1 - Pow(2 * x, 2))) * 0.5f : (Sqrt(1 - Pow(-2 * x + 2, 2)) + 1) * 0.5f;
        }

        public static float InBack(float x)
        {
            return c3 * x * x * x - c1 * x * x;
        }

        public static float OutBack(float x)
        {
            return 1 + c3 * Pow(x - 1, 3) + c1 * Pow(x - 1, 2);
        }

        public static float InOutBack(float x)
        {
            return x < 0.5f
                ? (Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) * 0.5f
                : (Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 1) * 0.5f;
        }

        public static float InElastic(float x)
        {
            switch (x)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return -Pow(2, 10 * x - 10) * Sin((x * 10 - 10.75f) * c4);
            }
        }
        
        public static float OutElastic(float x)
        {
            switch (x)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return -Pow(2, -10 * x) * Sin((x * 10 - 0.75f) * c4) + 1;
            }
        }
        
        public static float InOutElastic(float x)
        {
            switch (x)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                default:
                    return x < 0.5f 
                        ? -(Pow(2, 20 * x - 10) * Sin((20 * x - 11.125f) * c5)) * 0.5f 
                        : (Pow(2, -20 * x + 10) * Sin((20 * x - 11.125f) * c5)) * 0.5f + 1;
            }
        }

        public static float InBounce(float x)
        {
            return 1 - BounceOut(1 - x);
        }

        public static float OutBounce(float x)
        {
            return BounceOut(x);
        }

        public static float InOutBounce(float x)
        {
            return x < 0.5f
                ? (1 - BounceOut(1 - 2 * x)) * 0.5f
                : (1 + BounceOut(2 * x - 1)) * 0.5f;
        }

        public static float Linear(float x) => x;

        public static float Spring(float value)
        {
            return (Sin(value * PI * (0.2f + 2.5f * value * value * value)) * Pow(1f - value, 2.2f) + value) * (1f + (1f - value) * 1.2f);
        }
        
        private static float BounceOut(float x)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (x < 1 / d1) 
            {
                return n1 * x * x;
            }
            
            if (x < 2 / d1) 
            {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }
            
            if (x < 2.5 / d1) 
            {
                return n1 * (x -= 2.2f / d1) * x + 0.9375f;
            }
            
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
        
    }

}

