using UnityEngine;

public class GameplayLoseCanvas : BaseCanvas
{
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    public void Retry()
    {
        GameManager.Instance.LoadSceneQuick(GameScene.Gameplay, () =>
        {
            LevelManager.Instance.SpawnLevel(LevelManager.Instance.CurrentLevel);
        });
    }

    public void Quit()
    {
        GameManager.Instance.LoadSceneQuick(GameScene.Home, () =>
        {
            UIManager.Instance.Open<HomeCanvas>();
        });
    }
}
