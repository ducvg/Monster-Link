using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private MatchTileFactory[] matchTileFactories;

    void Awake()
    {
        foreach (var factory in matchTileFactories)
        {
            factory.PoolParent = new GameObject($"{factory.name} Pool").transform;
            factory.PoolParent.transform.position = Vector3.zero;
            factory.PoolParent.SetParent(transform);
        }

        DontDestroyOnLoad(this);
    }
}
