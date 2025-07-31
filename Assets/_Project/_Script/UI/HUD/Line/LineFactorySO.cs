using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "LineFactory", menuName = "Scriptable Objects/Line/Line Factory")]
public class LineFactorySO : ScriptableObject
{
    [SerializeField] private Line linePrefab;

    public Transform PoolParent {get => poolParent ;set => poolParent = value; }
    private Transform poolParent;

    protected Vector3 spawnPosition;
    protected IObjectPool<Line> pool;

    public virtual void OnInit()
    {

        if (pool == null)
        {
            pool = new ObjectPool<Line>(
                OnCreate,
                OnGet,
                OnRelease,
                OnDestroyLine,
                true, defaultCapacity: 2, maxSize: 5);
        }
    }

    public Line CreateLine(Vector3[] path)
    {

        var line = pool.Get();
        
        line.transform.position = Vector3.zero;
        line.ApplyPath(path);

        return line;
    }

    private Line OnCreate()
    {
        var line = Instantiate(linePrefab, spawnPosition, Quaternion.identity, poolParent);
        line.Pool = pool;

        return line;
    }

    private void OnGet(Line line)
    {
        line.gameObject.SetActive(true);
    }

    private void OnRelease(Line line)
    {
        line.gameObject.SetActive(false);
    }

    private void OnDestroyLine(Line tile)
    {
        Destroy(tile.gameObject);
    }
}
