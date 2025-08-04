using System;
using UnityEngine;
using UnityEngine.UI;
using Utility.SkibidiTween;

public class GameplaySettingCanvas : BaseCanvas
{
    [Header("Gameplay Setting Canvas Settings")]
    [SerializeField] private Slider backgroundVolumeSlider;
    [SerializeField] private Slider fxVolumeSlider;
    [SerializeField] private Button backgroundButton, fxButton;
    [SerializeField] private Sprite muteSprite, unmuteSprite;

    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    public override void Setup()
    {
        base.Setup();
        SetBackgroundVolume(SaveSystem.userData.settingConfig.BackgroundVolume);
        SetFxVolume(SaveSystem.userData.settingConfig.FxVolume);
        backgroundVolumeSlider.value = SaveSystem.userData.settingConfig.BackgroundVolume;
        fxVolumeSlider.value = SaveSystem.userData.settingConfig.FxVolume;
    }

    public void OnResumeClick()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        UIManager.Instance.Close<GameplaySettingCanvas>();

        GameState.OnGameResume?.Invoke();
    }

    public void OnQuitClick()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        playerDataProfile.Save();
        SaveSystem.Save();
        
        UIManager.Instance.Close<GameplaySettingCanvas>();
        GameState.OnGameLost?.Invoke();
        GameManager.Instance.LoadSceneQuick(GameScene.Home, () =>
        {
            UIManager.Instance.Open<HomeCanvas>();
            SoundManager.Instance.ChangeSound(SoundID.Home_BG, 1f);
        });
    }

    public void ToggleBackgroundVolume()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        if(Mathf.Approximately(SaveSystem.userData.settingConfig.BackgroundVolume, 0))
        {
            SetBackgroundVolume(1);
            backgroundVolumeSlider.value = 1;
        } else {
            SetBackgroundVolume(0);
            backgroundVolumeSlider.value = 0;
        }
    }

    public void ToggleFxVolume()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        if(Mathf.Approximately(SaveSystem.userData.settingConfig.FxVolume, 0))
        {
            SetFxVolume(1);
            fxVolumeSlider.value = 1;
        } else {
            SetFxVolume(0);
            fxVolumeSlider.value = 0;
        }
    }

    public void SetBackgroundVolume(Single value)
    {
        SaveSystem.userData.settingConfig.BackgroundVolume = value;
        SoundManager.Instance.UpdateSoundVolume();
        if (Mathf.Approximately(value, 0))
        {
            backgroundButton.image.sprite = muteSprite;
        } else {
            backgroundButton.image.sprite = unmuteSprite;
        }
    }

    public void SetFxVolume(Single value)
    {
        SaveSystem.userData.settingConfig.FxVolume = value;
        if (Mathf.Approximately(value, 0))
        {
            fxButton.image.sprite = muteSprite; 
        } else {
            fxButton.image.sprite = unmuteSprite;
        }
    }

}
