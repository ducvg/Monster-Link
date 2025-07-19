using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ItemTile Factory", menuName = "Scriptable Objects/Factory/MatchTile/ItemTile")]
public class ItemTileFactorySO : MatchTileFactory
{
    public override MatchTile CreateMatchTile(MatchTile matchTilePrefab, Vector3 position)
    {
        this.spawnPosition = position;
        this.matchTilePrefab = matchTilePrefab;

        var tile = pool.Get();
        tile.ApplyData(matchTilePrefab);
        return tile;
    }
}

