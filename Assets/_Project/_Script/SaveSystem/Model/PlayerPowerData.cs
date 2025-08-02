using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerPowerData
{
    [SerializeField] private int autoSolve = 10;
    [SerializeField] private int shuffle = 10;

    [JsonIgnore]
    public int AutoSolve { 
        get => autoSolve;
        set
        {
            autoSolve = value;
            GamePowerState.OnAutoSolveUpdate?.Invoke();
        }
    }

    [JsonIgnore]
    public int Shuffle { 
        get => shuffle;
        set
        {
            shuffle = value;
            GamePowerState.OnShuffleUpdate?.Invoke();
        }
    }

    public void UseAutoSolve()
    {
        if(!GamePowerState.isAllow) return;

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
        if(!GamePowerState.isAllow) return;

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