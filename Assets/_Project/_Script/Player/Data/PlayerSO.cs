using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataProfile", menuName = "Scriptable Objects/Player/Player Data Profile")]
public class PlayerDataProfileSO : ScriptableObject
{
    [field: SerializeField] public PlayerData PlayerData { get; private set; }
    [field: SerializeField] public PlayerPowerData PowerData { get; private set; }

    public void LoadPowerData()
    {
        PowerData = SaveSystem.gameData.powerData.CloneInstance();
    }

    public void Save()
    {
        SaveSystem.gameData.playerData = PlayerData.CloneInstance();
        SaveSystem.gameData.powerData = PowerData.CloneInstance();
    }

    public void Load()
    {
        PlayerData = SaveSystem.gameData.playerData.CloneInstance();
        PowerData = SaveSystem.gameData.powerData.CloneInstance();
    }
}


