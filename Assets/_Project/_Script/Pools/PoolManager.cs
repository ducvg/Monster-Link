using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private LineFactorySO lineFactory;

    protected void Awake()
    {


        lineFactory.OnInit();
        lineFactory.PoolParent = new GameObject($"Line Pool").transform;
        lineFactory.PoolParent.transform.position = Vector3.zero;
        lineFactory.PoolParent.SetParent(transform);
    }
    
}

