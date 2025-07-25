using TMPro;
using UnityEngine;

public class GameplayCanvas : BaseCanvas
{
    [SerializeField] private TextMeshProUGUI autoSolveTMP;
    [SerializeField] private TextMeshProUGUI shuffleTMP;

    [SerializeField] private PlayerDataSO playerData;

    public override void Setup()
    {
        base.Setup();

        autoSolveTMP.text = playerData.PowerData.AutoSolve.ToString();
        shuffleTMP.text = playerData.PowerData.Shuffle.ToString();
    }

    
}
