using UnityEngine;

[CreateAssetMenu(fileName = "AutoSolve", menuName = "Scriptable Objects/Tile Effect/Auto Solve")]
public class AutoSolveEffect : TileEffect
{
    [Header("AutoSolve effect Properties")]
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    public override void ApplyEffect()
    {
        playerDataProfile.PowerData.AutoSolve++;
    }

    public override void RemoveEffect()
    {
        
    }
}
