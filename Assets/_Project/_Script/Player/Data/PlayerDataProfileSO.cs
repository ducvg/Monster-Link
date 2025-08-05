using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataProfile", menuName = "Scriptable Objects/Player/Player Data Profile")]
public class PlayerDataProfileSO : ScriptableObject
{
    [field: SerializeField] public PlayerData PlayerData { get; private set; }
    [field: SerializeField] public PlayerPowerData PowerData { get; private set; }

    public void LoadPowerData()
    {
        PowerData = SaveSystem.userData.powerData.CloneInstance();
    }

    public void Save()
    {
        SaveSystem.userData.playerData = PlayerData.CloneInstance();
        SaveSystem.userData.powerData = PowerData.CloneInstance();
    }

    public void Load()
    {
        PlayerData = SaveSystem.userData.playerData.CloneInstance();
        PowerData = SaveSystem.userData.powerData.CloneInstance();
    }
}


