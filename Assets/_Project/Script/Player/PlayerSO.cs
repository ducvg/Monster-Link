using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player/Player Data")]
public class PlayerDataSO : ScriptableObject
{
#region Player Data fields
    [SerializeField] private int maxLevel;
    [SerializeField] private int playerGold;

    [SerializeField] private PlayerPowerSO powerData;
#endregion

    public int MaxLevel => maxLevel;
    public int PlayerGold => playerGold;
    public PlayerPowerSO PowerData => powerData;

    public void Save()
    {

    }

    public void Load()
    {

    }
}


