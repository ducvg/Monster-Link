using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private int maxCompletedLevel = 0;
    [SerializeField] private int lastPlayedLevel = 1;
    [SerializeField] private int playerGold = 0;

    [JsonIgnore]
    public int MaxCompletedLevel { 
        get => maxCompletedLevel;
        set
        {
            maxCompletedLevel = value;
        }
    }

    [JsonIgnore]
    public int LastPlayedLevel { 
        get => lastPlayedLevel;
        set
        {
            lastPlayedLevel = value;
        }
    }

    [JsonIgnore]
    public int PlayerGold { 
        get => playerGold;
        set
        {
            playerGold = value;
        }
    }
}
