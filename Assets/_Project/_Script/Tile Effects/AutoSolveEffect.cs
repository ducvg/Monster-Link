using UnityEngine;

[CreateAssetMenu(fileName = "AutoSolve", menuName = "Scriptable Objects/Tile Effect/Auto Solve")]
public class AutoSolveEffect : TileEffect
{
    [Header("AutoSolve effect Properties")]
    [SerializeField] private PlayerPowerSO playerPowerData;

    public override void ApplyEffect()
    {
        playerPowerData.AutoSolve++;
    }

    public override void RemoveEffect()
    {
        
    }
}
