using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GameTile Factory", menuName = "Scriptable Objects/Tile/Factory/GameTile")]
public class GameTileFactorySO : ScriptableObject
{
    [Header("MatchTile Factories")]
    public MatchTileFactory monsterTileFactory;
    public MatchTileFactory itemTileFactory;

    public MatchTile CreateMonsterTile(MatchTile matchTilePrefab, Vector3 position, Transform parent)
    {
        return monsterTileFactory.CreateTile(matchTilePrefab, position, parent);
    }

    public MatchTile CreateItemTile(MatchTile matchTilePrefab, Vector3 position, Transform parent)
    {
        return itemTileFactory.CreateTile(matchTilePrefab, position, parent);
    }
}
