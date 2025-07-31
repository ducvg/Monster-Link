using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayCanvas : BaseCanvas
{
    [Header("Gameplay Canvas Settings")]
    [SerializeField] private TextMeshProUGUI autoSolveTMP;
    [SerializeField] private TextMeshProUGUI shuffleTMP;
    [SerializeField] private TimerBarUI timerBar;

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
    }

    public void OnShuffleClick()
    {
        playerData.PowerData.UseShuffle();
    }

    public void OnInventoryClick()
    {

    }

    public void OnSettingClick()
    {
        GameState.OnGamePause?.Invoke();

        UIManager.Instance.Open<GameplaySettingCanvas>();
    }

    private void UpdateShuffleTMP()
    {
        shuffleTMP.text = playerData.PowerData.Shuffle.ToString();
    }

    private void UpdateAutoSolveTMP()
    {
        autoSolveTMP.text = playerData.PowerData.AutoSolve.ToString();
    }

    void EnableInteract()
    {
        canvasGroup.interactable = true;
    }

    void DisableInteract()
    {
        canvasGroup.interactable = false;
    }

    void OnEnable()
    {
        GamePowerState.OnShuffleUpdate += UpdateShuffleTMP;
        GamePowerState.OnAutoSolveUpdate += UpdateAutoSolveTMP;
        GameState.OnGamePause += DisableInteract;
        GameState.OnGameResume += EnableInteract;
    }

    void OnDisable()
    {
        GamePowerState.OnShuffleUpdate -= UpdateShuffleTMP;
        GamePowerState.OnAutoSolveUpdate -= UpdateAutoSolveTMP;
        GameState.OnGamePause -= DisableInteract;
        GameState.OnGameResume -= EnableInteract;
        
    }
}
