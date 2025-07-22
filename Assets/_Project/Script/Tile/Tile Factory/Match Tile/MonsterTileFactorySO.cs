using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTile Factory", menuName = "Scriptable Objects/Tile/Factory/MatchTile/MonsterTile")]
public class MonsterTileFactorySO : MatchTileFactory
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