using UnityEngine;
using UnityEngine.Pool;

public abstract class MatchTileFactory : ScriptableObject
{
    public Transform PoolParent
    {
        get => poolParent;
        set => poolParent = value;
    }
    protected Transform poolParent;
    protected IObjectPool<MatchTile> pool;
    protected MatchTile matchTilePrefab;
    protected Vector3 spawnPosition;

    protected virtual void OnEnable()
    {

        if (pool == null)
        {
            pool = new ObjectPool<MatchTile>(
                OnCreateTile,
                OnGetTile,
                OnReleaseTile,
                OnDestroyTile,
                true, defaultCapacity: 90, maxSize: 300);
        }
    }

    public abstract MatchTile CreateMatchTile(MatchTile matchTilePrefab, Vector3 position);

    private MatchTile OnCreateTile()
    {
        var tile = Instantiate(matchTilePrefab, spawnPosition, Quaternion.identity, poolParent);
        tile.Pool = pool;
        tile.OnInit();
        return tile;
    }

    private void OnGetTile(MatchTile tile)
    {
        tile.gameObject.SetActive(true);
        tile.transform.position = spawnPosition;
        tile.OnInit();
    }

    private void OnReleaseTile(MatchTile tile)
    {
        tile.OnDespawn();
        tile.gameObject.SetActive(false);
    }

    private void OnDestroyTile(MatchTile tile)
    {
        tile.OnDespawn();
        Destroy(tile.gameObject);
    }
}
