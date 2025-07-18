using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GameTile Factory", menuName = "Scriptable Objects/Factory/GameTile")]
public class GameTileFactorySO : ScriptableObject
{
    [Header("MatchTile Factories")]
    public MatchTileFactory monsterTileFactory;
    public MatchTileFactory itemTileFactory;

    [Header("ObstacleTile Factories")]
    public ObstacleTileFactory waterTileFactory;
    public ObstacleTileFactory rockTileFactory;

    public MatchTile CreateMonsterTile(MatchTile matchTilePrefab)
    {
        return monsterTileFactory.CreateMatchTile(matchTilePrefab);
    }

    public MatchTile CreateItemTile(MatchTile matchTilePrefab)
    {
        return itemTileFactory.CreateMatchTile(matchTilePrefab);
    }

    public ObstacleTile CreateWaterTile(ObstacleTile obstacleTilePrefab)
    {
        return waterTileFactory.CreateObstacleTile(obstacleTilePrefab);
    }

    public ObstacleTile CreateRockTile(ObstacleTile obstacleTilePrefab)
    {
        return rockTileFactory.CreateObstacleTile(obstacleTilePrefab);
    }

}
