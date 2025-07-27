using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayCanvas : BaseCanvas
{
    [Header("Gameplay Canvas Settings")]
    [SerializeField] private TextMeshProUGUI autoSolveTMP;
    [SerializeField] private TextMeshProUGUI shuffleTMP;
    [SerializeField] private TimerBar timerBar;

    [SerializeField] private PlayerDataSO playerData;

    public void OnInit()
    {
        timerBar.OnInit();
        autoSolveTMP.text = playerData.PowerData.AutoSolve.ToString();
        shuffleTMP.text = playerData.PowerData.Shuffle.ToString();
    }

    public void OnAutoSolveClick()
    {
        playerData.PowerData.UseAutoSolve();
        autoSolveTMP.text = playerData.PowerData.AutoSolve.ToString();
    }
    
    public void OnShuffleClick()
    {
        playerData.PowerData.UseShuffle();
        shuffleTMP.text = playerData.PowerData.Shuffle.ToString();
    }

    public void OnInventoryClick()
    {

    }

    public void OnSettingClick()
    {
        GameState.OnGamePause?.Invoke(); //Invoke before disable timer

        UIManager.Instance.Open<GameplaySettingCanvas>();
    }

    void OnEnable()
    {
        GameState.OnGamePause += DisableInteract;
        GameState.OnGameResume += EnableInteract;
    }

    void OnDisable()
    {
        GameState.OnGamePause -= DisableInteract;
        GameState.OnGameResume -= EnableInteract;
    }

    void EnableInteract()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }

    void DisableInteract()
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.interactable = false;
    }
}
