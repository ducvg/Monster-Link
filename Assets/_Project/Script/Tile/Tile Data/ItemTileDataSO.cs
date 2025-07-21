using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Objects/Tile/Data/Item Tile")]
public class ItemTileDataSO : MatchTileData
{
    [Header("Item Tile Properties")]
    public string itemName;
}

