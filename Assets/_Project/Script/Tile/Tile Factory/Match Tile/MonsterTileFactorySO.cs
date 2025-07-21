using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "MonsterTile Factory", menuName = "Scriptable Objects/Tile/Factory/MatchTile/MonsterTile")]
public class MonsterTileFactorySO : MatchTileFactory
{

    public override MatchTile CreateMatchTile(MatchTile matchTilePrefab, Vector3 position)
    {
        this.spawnPosition = position;
        this.matchTilePrefab = matchTilePrefab;

        var tile = pool.Get();
        tile.CopyFrom(matchTilePrefab);
        return tile;
    }


}