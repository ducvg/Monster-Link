using UnityEngine;

[CreateAssetMenu(fileName = "Power Data", menuName = "Scriptable Objects/Player/Power Data")]
public class PlayerPowerSO : ScriptableObject
{
#region Power Data fields
    [SerializeField] private int autoSolve = 10;
    [SerializeField] private int shuffle = 10;
#endregion

    public int AutoSolve { get => autoSolve; set => autoSolve = value; }
    public int Shuffle { get => shuffle; set => shuffle = value; }

    public void UseAutoSolve()
    {
        if (AutoSolve > 0)
        {
            BoardManager.Instance.AutoSolve();
            AutoSolve--;
        } else
        {
            Debug.Log("No auto solve power left");
        }
    }

    public void UseShuffle()
    {
        if (Shuffle > 0)
        {
            BoardManager.Instance.Shuffle();
            Shuffle--;
        } else
        {
            Debug.Log("No shuffle power left");
        }
    }
} 


