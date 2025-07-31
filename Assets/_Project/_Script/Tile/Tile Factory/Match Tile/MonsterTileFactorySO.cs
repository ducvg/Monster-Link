using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTile Factory", menuName = "Scriptable Objects/Tile/Factory/Match Tile/Monster Tile")]
public class MonsterTileFactorySO : MatchTileFactory
{

    public override MatchTile CreateTile(MatchTile matchTilePrefab, Vector3 position, Transform parent)
    {
        this.spawnPosition = position;
        this.matchTilePrefab = matchTilePrefab;

        MatchTile tile;
        tile = Instantiate(matchTilePrefab, position, Quaternion.identity, parent);
        tile.Pool = null;
        
        return tile;
    }


}