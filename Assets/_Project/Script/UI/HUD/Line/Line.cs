using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer outerLine, innerLine;

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
        yield return new WaitForSeconds(delay);

        pool.Release(this);
    }
}
