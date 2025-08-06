using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//my tweening library
#pragma warning disable IDE0130 // Shut the ide up
namespace Utility.SkibidiTween
#pragma warning restore IDE0130
{
    public static class SkibidiTween
    {
        public static IEnumerator SkibidiMoveAtSpeed(this Transform target, Vector3 end, float speed, Action onComplete = null, Action onUpdate = null, bool isReverse = false)
        {
            Vector3 startPos = isReverse ? end : target.position;
            Vector3 endPos = isReverse ? target.position : end;

            target.position = startPos;

            while ((target.position - endPos).sqrMagnitude > 0.0001f)
            {
                target.position = Vector3.MoveTowards(target.position, endPos, Time.deltaTime * speed);
                onUpdate?.Invoke();
                yield return null;
            }
            target.position = endPos;

            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiMove(this Transform target, Skibidi<Vector2> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            float elapsed = 0f;
            float duration = tween.duration;
            Vector2 start = isReverse ? tween.end : tween.start; //cache
            Vector2 end = isReverse ? tween.start : tween.end; 

            target.position = start;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = tween.easeCurve.Evaluate(elapsed / duration);
                target.position = Vector2.LerpUnclamped(start, end, t);
                yield return null;
            }
            target.position = end;
            
            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiMove(this RectTransform target, Skibidi<Vector2> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            float duration = tween.duration;
            Vector2 start = isReverse ? tween.end : tween.start;
            Vector2 end = isReverse ? tween.start : tween.end;

            target.anchoredPosition = start;
            
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = tween.easeCurve.Evaluate(elapsed / duration);
                target.anchoredPosition = Vector2.LerpUnclamped(start, end, t);
                yield return null;
            }
            target.anchoredPosition = end;
            
            onComplete?.Invoke();
        }

        public static IEnumerator OnComplete(this IEnumerator coroutine, Action onComplete)
        {
            yield return coroutine;
            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiFade(this CanvasGroup target, Skibidi<float> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            float duration = tween.duration;
            float start = isReverse ? tween.end : tween.start;
            float end = isReverse ? tween.start : tween.end;

            target.alpha = start;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = tween.easeCurve.Evaluate(elapsed / duration);
                target.alpha = Mathf.Lerp(start, end, t);
                yield return null;
            }
            target.alpha = end;

            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiColor(this Graphic target, Skibidi<Color> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            float duration = tween.duration;
            Color start = isReverse ? tween.end : tween.start;
            Color end = isReverse ? tween.start : tween.end;
            
            target.color = start;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = tween.easeCurve.Evaluate(elapsed / duration);
                target.color = Color.LerpUnclamped(start, end, t);
                yield return null;
            }
            target.color = end;

            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiRotate(this Transform target, Skibidi<Vector3> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            float duration = tween.duration;
            Vector3 start = isReverse ? tween.end : tween.start;
            Vector3 end = isReverse ? tween.start : tween.end;

            target.rotation = Quaternion.Euler(start);

            float elapsed = 0f;
            Vector3 currentRotation = start;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = tween.easeCurve.Evaluate(elapsed / duration);
                currentRotation = Vector3.LerpUnclamped(start, end, t);
                target.rotation = Quaternion.Euler(currentRotation);
                yield return null;
            }
            target.rotation = Quaternion.Euler(end);

            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiRotateLoop(this Transform target, Skibidi<Vector3> tween, bool isPingPong = false)
        {
            if (isPingPong)
            {
                while (true)
                {
                    yield return target.SkibidiRotate(tween, null, isReverse: false);
                    yield return target.SkibidiRotate(tween, null, isReverse: true);
                }
            }
            else
            {
                while (true)
                {
                    yield return target.SkibidiRotate(tween, null, isReverse: false);
                }
            }
        }


        public static IEnumerator SkibidiScale(this Transform target, Skibidi<Vector3> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            float duration = tween.duration;
            Vector3 start = isReverse ? tween.end : tween.start;
            Vector3 end = isReverse ? tween.start : tween.end;

            target.localScale = start;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = tween.easeCurve.Evaluate(elapsed / duration);
                target.localScale = Vector3.LerpUnclamped(start, end, t);
                yield return null;
            }
            target.localScale = end;

            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiScaleLoop(this Transform target, Skibidi<Vector3> tween, bool isPingPong = false)
        {
            if(isPingPong)
            {
                while(true)
                {
                    yield return target.SkibidiScale(tween, null, isReverse: false);
                    yield return target.SkibidiScale(tween, null, isReverse: true);
                }
            }
            else
            {
                while(true)
                {
                    yield return target.SkibidiScale(tween, null, isReverse: false);
                }
            }
        }

        public static IEnumerator SkibidiDelay(float duration, Action onComplete = null)
        {
            if(duration > 0) yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
        }
    }

    [Serializable]
    public class Skibidi<T>
    {
        public bool isUse = false;

        public T start;
        public T end;

        public float duration;
        public AnimationCurve easeCurve;
    }
}


