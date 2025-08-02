using System;
using UnityEngine;

[Serializable]
public static class SaveSystem
{
    public static bool isCorrupted = false;
    public static GameData gameData = new();

    public static void Save()
    {
        if (!isCorrupted)
        {
            FileService.SaveLocal("/save.sav",gameData, false);
        } else
        {
            Debug.LogError("Game data is corrupted, save disable.");
        }
    }

    public static GameData Load()
    {
        try
        {
            gameData = FileService.LoadLocal<GameData>("/save.sav", false);
            if (gameData == null)
            {
                gameData = new();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load game data: " + e.Message);
            gameData = new();
        }
        
        return gameData;
    }

}

public class GameData 
{
    public PlayerData playerData = new();
    public PlayerPowerData powerData = new();
}