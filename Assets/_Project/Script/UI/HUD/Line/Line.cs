using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;
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
        lifeTime = delay;
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(lifeTime);

        pool.Release(this);
    }
}
