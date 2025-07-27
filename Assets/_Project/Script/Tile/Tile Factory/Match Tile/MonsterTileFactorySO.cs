using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTile Factory", menuName = "Scriptable Objects/Tile/Factory/Match Tile/Monster Tile")]
public class MonsterTileFactorySO : MatchTileFactory
{

    public override MatchTile CreateTile(MatchTile matchTilePrefab, Vector3 position, Transform parent)
    {
        this.spawnPosition = position;
        this.matchTilePrefab = matchTilePrefab;

        MatchTile tile;
        if(isUsePool)
        {
            tile = pool.Get();
            tile.OnInit();
            tile.CopyFrom(matchTilePrefab);
        }
        else
        {
            tile = Instantiate(matchTilePrefab, position, Quaternion.identity, parent);
            tile.Pool = null;
        }
        
        return tile;
    }


}