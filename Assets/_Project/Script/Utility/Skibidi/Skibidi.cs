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
        public static IEnumerator SkibidiMove(this Transform target, Skibidi<Vector3> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) yield break;

            target.position = isReverse ? tween.end : tween.start;

            float elapsed = 0f;
            float duration = tween.duration;
            Vector3 start = isReverse ? tween.end : tween.start;
            Vector3 end = isReverse ? tween.start : tween.end;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = tween.easeCurve.Evaluate(elapsed / duration);
                target.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiFadeCanvas(this CanvasGroup target, Skibidi<float> tween, Action onComplete = null, bool isReverse = false)
        {
            if(!tween.isUse) yield break;

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
            onComplete?.Invoke();
        }

        public static IEnumerator SkibidiDelay(float duration, Action onComplete = null)
        {
            yield return new WaitForSeconds(duration);
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


