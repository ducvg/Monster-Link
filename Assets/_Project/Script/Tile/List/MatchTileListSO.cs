using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchTile List", menuName = "Scriptable Objects/Storage/List/MatchTile List")]
public class MatchTileListSO : ScriptableObject
{
    public List<MatchTile> monsterTiles;
    public List<MatchTile> itemTiles;
}
