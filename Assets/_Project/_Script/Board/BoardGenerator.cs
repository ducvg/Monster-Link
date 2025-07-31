using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardGenerator
{
    [SerializeField] private SpawnSetting[] tileSpawnGroups;
    [SerializeField] private EffectSpawnSetting[] effectSpawnGroups;
    
    public void FillBoard()
    {
        List<(int x, int y)> unfilledPositions = GameBoard.GetEmptyPositions();
        var tilemap = BoardManager.Instance.BoardTilemap;

        (int x, int y) randomPos;
        Vector3 position;
        Vector3Int tilePos = Vector3Int.zero;
        var emptyCount = unfilledPositions.Count;
        foreach (var spawnGroup in tileSpawnGroups)
        {
            int tilesToSpawnCount = (int)(emptyCount * spawnGroup.groupChance);
            tilesToSpawnCount += tilesToSpawnCount % 2; // ensure even number for pair spawning
            var factory = spawnGroup.factory;
            int tileIndex = 0;

            while (tilesToSpawnCount >= 2)
            {
                if (unfilledPositions.Count >= 2) //place by pairs
                {
                    randomPos = unfilledPositions.GetRandomElement();
                    tilePos.x = randomPos.x;
                    tilePos.y = randomPos.y;
                    position = tilemap.GetCellCenterWorld(tilePos);
                    factory.CreateTile(spawnGroup.tiles[tileIndex], position, tilemap.transform);
                    unfilledPositions.Remove(randomPos);
                    tilesToSpawnCount--;

                    randomPos = unfilledPositions.GetRandomElement();
                    tilePos.x = randomPos.x;
                    tilePos.y = randomPos.y;
                    position = tilemap.GetCellCenterWorld(tilePos);
                    factory.CreateTile(spawnGroup.tiles[tileIndex], position, tilemap.transform);
                    unfilledPositions.Remove(randomPos);
                    tilesToSpawnCount--;

                    tileIndex++;
                    if(tileIndex >= spawnGroup.tiles.Length)
                    {
                        tileIndex = 0;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }   

    public void GenerateTileEffects()
    {
        List<MatchTile> matchTiles = GameBoard.GetMatchTiles();

        foreach (var spawnGroup in effectSpawnGroups)
        {
            while (spawnGroup.spawnAmount > 0)
            {
                var randomTile = matchTiles.GetRandomElement();
                if(randomTile.TileEffect != null) continue;
                
                var effect = spawnGroup.effect;
                randomTile.TileEffect = effect;
                effect.OnInit(randomTile);
                spawnGroup.spawnAmount--;
            }
        }
    }
}

[Serializable]
public class SpawnSetting
{
    public MatchTile[] tiles;
    [Range(0, 1)] public float groupChance;
    public MatchTileFactory factory;
}

[Serializable]
public class EffectSpawnSetting
{
    public TileEffect effect;
    public int spawnAmount;
}

