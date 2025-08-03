using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeSettingCanvas : BaseCanvas
{
    [Header("Home Setting Canvas Settings")]
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
    }

    public override void Close()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        base.Close();
    }

    public void Quit()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        playerDataProfile.Save();
        SaveSystem.Save();
        Application.Quit();
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

    public void ToggleBackgroundVolume()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        if(Mathf.Approximately(SaveSystem.userData.settingConfig.BackgroundVolume, 0))
        {
            SetBackgroundVolume(1);
        } else {
            SetBackgroundVolume(0);
        }
    }

    public void ToggleFxVolume()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        if(Mathf.Approximately(SaveSystem.userData.settingConfig.FxVolume, 0))
        {
            SetFxVolume(1);
        } else {
            SetFxVolume(0);
        }
    }
}
