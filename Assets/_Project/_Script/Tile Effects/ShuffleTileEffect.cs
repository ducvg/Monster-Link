using UnityEngine;

[CreateAssetMenu(fileName = "Shuffle", menuName = "Scriptable Objects/Tile Effect/Shuffle")]
public class ShuffleEffect : TileEffect
{
    [Header("Shuffle effect Properties")]
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    public override void ApplyEffect()
    {
        playerDataProfile.PowerData.Shuffle++;
    }

    public override void RemoveEffect()
    {
        
    }
}
