using UnityEngine;

[CreateAssetMenu(fileName = "ItemTile Factory", menuName = "Scriptable Objects/Tile/Factory/MatchTile/ItemTile")]
public class ItemTileFactorySO : MatchTileFactory
{
    
    public override MatchTile CreateTile(MatchTile matchTilePrefab, Vector3 position)
    {
        this.spawnPosition = position;
        this.matchTilePrefab = matchTilePrefab;

        var tile = pool.Get();
        tile.CopyFrom(matchTilePrefab);

        return tile;
    }
}

