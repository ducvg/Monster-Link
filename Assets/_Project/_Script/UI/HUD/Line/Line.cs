using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer outerLine, innerLine;
    [SerializeField] private AnimationCurve lerpCurve;

    public IObjectPool<Line> Pool { set => pool = value; }
    private IObjectPool<Line> pool;

    public void ApplyPath(Vector3[] path)
    {
        outerLine.positionCount = path.Length;
        outerLine.SetPositions(path);

        innerLine.positionCount = path.Length;
        innerLine.SetPositions(path);
    }

    public void OnDespawn(float delay = 1f)
    {
        StartCoroutine(Deactivate(delay));
    }

    private IEnumerator Deactivate(float delay)
    {
        var outerWidth = 0.12f;
        var innerWidth = 0.055f;
        var outerColor = Color.black;
        var innerColor = Color.limeGreen;

        var elapsed = 0f;
        while(elapsed < delay)
        {
            float t = lerpCurve.Evaluate(elapsed / delay);

            elapsed += Time.deltaTime;
            outerLine.startWidth = outerLine.endWidth = Mathf.Lerp(outerWidth, 0, t);
            innerLine.startWidth = innerLine.endWidth = Mathf.Lerp(innerWidth, 0, t);

            outerLine.startColor = outerLine.endColor = Color.Lerp(outerColor, Color.clear, t);
            innerLine.startColor = innerLine.endColor = Color.Lerp(innerColor, Color.clear, t);

            yield return null;
        }

        outerLine.startWidth = outerLine.endWidth = outerWidth;
        outerLine.startColor = outerLine.endColor = outerColor;

        innerLine.startWidth = innerLine.endWidth = innerWidth;
        innerLine.startColor = innerLine.endColor = innerColor;

        pool.Release(this);
    }
}
