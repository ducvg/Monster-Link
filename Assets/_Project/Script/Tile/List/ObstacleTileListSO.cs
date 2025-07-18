using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleTile List", menuName = "Scriptable Objects/Storage/List/ObstacleTile List")]
public class ObstacleTileListSO : ScriptableObject
{
    public List<ObstacleTile> waterTiles;
    public List<ObstacleTile> rockTiles;
}
