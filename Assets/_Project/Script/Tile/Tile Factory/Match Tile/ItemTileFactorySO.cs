using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ItemTile Factory", menuName = "Scriptable Objects/Factory/MatchTile/ItemTile")]
public class ItemTileFactorySO : MatchTileFactory
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if(poolParent == null)
        {
            // Create a parent GameObject for the pool if it doesn't exist
            poolParent = new GameObject("ItemTile Pool").transform;
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

