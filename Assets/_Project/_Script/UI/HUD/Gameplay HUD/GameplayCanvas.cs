using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : BaseCanvas
{
    [SerializeField] private TextMeshProUGUI autoSolveTMP;
    [SerializeField] private TextMeshProUGUI shuffleTMP;
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    private void Start()
    {
        Setup();
    }

    public override void Setup()
    {
        base.Setup();
        canvasGroup.interactable = true;

        autoSolveTMP.text = playerDataProfile.PowerData.AutoSolve.ToString();
        shuffleTMP.text = playerDataProfile.PowerData.Shuffle.ToString();
    }

    public void OnAutoSolveClick()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        playerDataProfile.PowerData.UseAutoSolve();
    }

    public void OnShuffleClick()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        playerDataProfile.PowerData.UseShuffle();
    }

    public void OnInventoryClick()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
    }

    public void OnSettingClick()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
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
        GameState.OnGameWon += DisableInteract;
        GameState.OnGamePause += DisableInteract;
        GameState.OnGameResume += EnableInteract;
    }

    void OnDisable()
    {
        GamePowerState.OnShuffleUpdate -= UpdateShuffleTMP;
        GamePowerState.OnAutoSolveUpdate -= UpdateAutoSolveTMP;
        GameState.OnGameWon -= DisableInteract;
        GameState.OnGamePause -= DisableInteract;
        GameState.OnGameResume -= EnableInteract;
        
    }
}
