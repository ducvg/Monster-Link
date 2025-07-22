using UnityEngine;

public class PoolManager : PersistentSingleton<PoolManager>
{
    [SerializeField] private MatchTileFactory[] matchTileFactories;
    [SerializeField] private LineFactorySO lineFactory;

    protected override void Awake()
    {
        base.Awake();

        foreach (var factory in matchTileFactories)
        {
            factory.PoolParent = new GameObject($"{factory.name} Pool").transform;
            factory.PoolParent.transform.position = Vector3.zero;
            factory.PoolParent.SetParent(transform);
        }
        lineFactory.PoolParent = new GameObject($"Line Pool").transform;
        lineFactory.PoolParent.transform.position = Vector3.zero;
        lineFactory.PoolParent.SetParent(transform);
    }
}
