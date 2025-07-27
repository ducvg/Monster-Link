using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchTile List", menuName = "Scriptable Objects/Storage/MatchTile List")]
public class MatchTileListSO : ScriptableObject
{
    public List<MatchTile> monsterTiles;
    public List<MatchTile> itemTiles;

    public int monsterTileIndex = 0;

    public MatchTile GetCurrentMonsterTile()
    {
        return monsterTiles[monsterTileIndex];
    }

    public void NextMonsterIndex()
    {
        monsterTileIndex++;
        if (monsterTileIndex >= monsterTiles.Count)
        {
            monsterTileIndex = 0;
            monsterTiles.Shuffle();
        }
    }
}
