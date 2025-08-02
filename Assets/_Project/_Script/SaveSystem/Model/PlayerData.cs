using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private int maxLevel = 0;
    [SerializeField] private int lastPlayedLevel = 1;
    [SerializeField] private int playerGold = 0;

    [JsonIgnore]
    public int MaxLevel { 
        get => maxLevel;
        set
        {
            maxLevel = value;
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
