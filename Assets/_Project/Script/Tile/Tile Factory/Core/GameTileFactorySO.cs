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

    public MatchTile CreateMonsterTile(MatchTile matchTilePrefab, Vector3 position)
    {
        return monsterTileFactory.CreateMatchTile(matchTilePrefab, position);
    }

    public MatchTile CreateItemTile(MatchTile matchTilePrefab, Vector3 position)
    {
        return itemTileFactory.CreateMatchTile(matchTilePrefab, position);
    }

    public ObstacleTile CreateWaterTile(ObstacleTile obstacleTilePrefab, Vector3 position)
    {
        return waterTileFactory.CreateObstacleTile(obstacleTilePrefab, position);
    }

    public ObstacleTile CreateRockTile(ObstacleTile obstacleTilePrefab, Vector3 position)
    {
        return rockTileFactory.CreateObstacleTile(obstacleTilePrefab, position);
    }

}
