using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Objects/Tile/Data/Monster Tile")]
public class MonsterTileDataSO : MatchTileData
{
    [Header("Monster Tile Properties")]
    public GameObject monsterPrefab;
}

