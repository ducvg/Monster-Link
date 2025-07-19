using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GameTile Factory", menuName = "Scriptable Objects/Factory/GameTile")]
public class GameTileFactorySO : ScriptableObject
{
    [Header("MatchTile Factories")]
    public MatchTileFactory monsterTileFactory;
    public MatchTileFactory itemTileFactory;

    public MatchTile CreateMonsterTile(MatchTile matchTilePrefab, Vector3 position)
    {
        return monsterTileFactory.CreateMatchTile(matchTilePrefab, position);
    }

    public MatchTile CreateItemTile(MatchTile matchTilePrefab, Vector3 position)
    {
        return itemTileFactory.CreateMatchTile(matchTilePrefab, position);
    }
}
