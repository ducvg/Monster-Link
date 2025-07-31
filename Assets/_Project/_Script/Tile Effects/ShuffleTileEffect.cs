using UnityEngine;

[CreateAssetMenu(fileName = "Shuffle", menuName = "Scriptable Objects/Tile Effect/Shuffle")]
public class ShuffleEffect : TileEffect
{
    [Header("Shuffle effect Properties")]
    [SerializeField] private PlayerPowerSO playerPowerData;

    public override void ApplyEffect()
    {
        playerPowerData.Shuffle++;
    }

    public override void RemoveEffect()
    {
        
    }
}
