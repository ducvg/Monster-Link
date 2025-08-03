using System.Collections;
using UnityEngine;

public class GameplayLoseCanvas : BaseCanvas
{
    [Header("Gameplay Lose Canvas Settings")]
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    public override void Setup()
    {
        base.Setup();
        SoundManager.Instance.StopSound();
        SoundManager.Instance.PlayFx(FxID.Game_Lose);
        
        playerDataProfile.Save();
        SaveSystem.Save();
    }

    public void Retry()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        GameManager.Instance.LoadSceneQuick(GameScene.Gameplay, () =>
        {
            LevelManager.Instance.SpawnLevel(LevelManager.Instance.CurrentLevel);
            SoundManager.Instance.ChangeSound(SoundID.Gameplay_BG, 1f);
        });
    }

    public void Quit()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        GameManager.Instance.LoadSceneQuick(GameScene.Home, () =>
        {
            UIManager.Instance.Open<HomeCanvas>();
            SoundManager.Instance.ChangeSound(SoundID.Home_BG, 1f);
        });
    }
}
