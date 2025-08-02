using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : BaseCanvas
{
    [Header("Gameplay Canvas Settings")]
    [SerializeField] private TextMeshProUGUI autoSolveTMP;
    [SerializeField] private TextMeshProUGUI shuffleTMP;
    [SerializeField] private TimerBarUI timerBar;

    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    public void OnInit()
    {
        timerBar.OnInit();

        autoSolveTMP.text = playerDataProfile.PowerData.AutoSolve.ToString();
        shuffleTMP.text = playerDataProfile.PowerData.Shuffle.ToString();
    }

    public void OnAutoSolveClick()
    {
        playerDataProfile.PowerData.UseAutoSolve();
    }

    public void OnShuffleClick()
    {
        playerDataProfile.PowerData.UseShuffle();
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
        shuffleTMP.text = playerDataProfile.PowerData.Shuffle.ToString();
    }

    private void UpdateAutoSolveTMP()
    {
        autoSolveTMP.text = playerDataProfile.PowerData.AutoSolve.ToString();
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
