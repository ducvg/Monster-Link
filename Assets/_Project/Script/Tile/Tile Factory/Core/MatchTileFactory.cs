using UnityEngine;
using UnityEngine.Pool;

public abstract class MatchTileFactory : ScriptableObject
{
    public Transform PoolParent {get; set;}
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

    public abstract MatchTile CreateTile(MatchTile matchTilePrefab, Vector3 position);

    private MatchTile OnCreateTile()
    {
        var tile = Instantiate(matchTilePrefab, spawnPosition, Quaternion.identity, PoolParent);
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
        tile.gameObject.SetActive(false);
    }

    private void OnDestroyTile(MatchTile tile)
    {
        Destroy(tile.gameObject);
    }
}
