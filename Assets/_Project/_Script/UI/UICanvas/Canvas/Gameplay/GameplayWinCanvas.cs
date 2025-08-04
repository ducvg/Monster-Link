using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWinCanvas : BaseCanvas
{
    [Header("Gameplay Win Canvas Settings")]
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    public override void Setup()
    {
        base.Setup();
        SoundManager.Instance.PlayFx(FxID.Game_Win);

        playerDataProfile.Save();
        SaveSystem.Save();
    }

    public void Claim()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        GameManager.Instance.LoadSceneQuick(GameScene.Home, () =>
        {
            UIManager.Instance.Open<HomeCanvas>();
            SoundManager.Instance.ChangeSound(SoundID.Home_BG, 1f);
        });
    }
}
