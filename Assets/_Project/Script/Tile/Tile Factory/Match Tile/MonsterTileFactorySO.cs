using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "MonsterTile Factory", menuName = "Scriptable Objects/Factory/MatchTile/MonsterTile")]
public class MonsterTileFactorySO : MatchTileFactory
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if(poolParent == null)
        {
            // Create a parent GameObject for the pool if it doesn't exist
            poolParent = new GameObject("MonsterTile Pool").transform;
        }        
    }

    public override MatchTile CreateMatchTile(MatchTile matchTilePrefab)
    {
        this.matchTilePrefab = matchTilePrefab;
        var tile = pool.Get();
        tile.ApplyVisual(matchTilePrefab);
        return tile;
    }


}