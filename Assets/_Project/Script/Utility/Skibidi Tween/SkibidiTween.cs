using System;
using System.Collections;
using UnityEngine;

//TODO: Implement SkibidiTween, a tweening library
#pragma warning disable IDE0130 // Shut the ide up
namespace Utility.SkibidiTween
#pragma warning restore IDE0130
{
    public static class SkibidiTween
    {
        public static IEnumerator SkibidiMove(this Transform target, Skibidi<Vector2> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            target.position = isReverse ? tween.end : tween.start;

            float elapsed = 0f;
            float duration = tween.duration;
            Vector2 start = isReverse ? tween.end : tween.start;
            Vector2 end = isReverse ? tween.start : tween.end;

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

            target.anchoredPosition = isReverse ? tween.end : tween.start;

            float elapsed = 0f;
            float duration = tween.duration;
            Vector2 start = isReverse ? tween.end : tween.start;
            Vector2 end = isReverse ? tween.start : tween.end;

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

        public static IEnumerator SkibidiFade(this CanvasGroup target, Skibidi<float> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) 
            {
                onComplete?.Invoke();
                yield break;
            }

            target.alpha = isReverse ? tween.end : tween.start;

            float elapsed = 0f;
            float duration = tween.duration;
            float start = isReverse ? tween.end : tween.start;
            float end = isReverse ? tween.start : tween.end;

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

        public static IEnumerator SkibidiDelay(float duration, Action onComplete = null)
        {
            if(duration > 0) yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
        }
    }

    [Serializable]
    public class Skibidi<T>
    {
        public bool isUse = true;

        public T start;
        public T end;

        public float duration;
        public AnimationCurve easeCurve;
    }
}


