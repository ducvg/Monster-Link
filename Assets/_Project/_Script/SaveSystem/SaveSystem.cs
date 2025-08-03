using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public static class SaveSystem
{
    public static bool isCorrupted = false;
    public static UserData userData = new();

    public static async Task SaveAsync()
    {
        var task = Task.Run(() => 
        {
            Save();
        });
        await task;
    }

    public static void Save()
    {
        if(isCorrupted)
        {
            Debug.LogError("Game data is corrupted, save disable.");
            return;
        }
        FileService.SaveLocal("/save.sav",userData, false);
    }
    
    public static UserData Load()
    {
        try
        {
            userData = FileService.LoadLocal<UserData>("/save.sav", false);
            if (userData == null)
            {
                userData = new();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load game data: " + e.Message);
            userData = new();
        }
        
        return userData;
    }

}

public class UserData 
{
    public PlayerData playerData = new();
    public PlayerPowerData powerData = new();
    public SettingConfig settingConfig = new();
}
