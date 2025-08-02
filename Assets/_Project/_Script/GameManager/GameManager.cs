using System;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public PlayerDataProfileSO PlayerDataProfile { get => playerDataProfile; }
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    void Start()
    {
        LoadScene(GameScene.Home,
        onComplete:() =>
        {
            LevelManager.Instance.OnInit();
        }
        );
        SaveSystem.Load();
        playerDataProfile.Load();
    }

    public void LoadScene(GameScene loadScene, Action onComplete = null)
    {
        StartCoroutine(SceneLoader.LoadScene
        (
            loadScene, 
            onComplete
        ));
    }

    public void LoadSceneQuick(GameScene loadScene, Action onComplete = null)
    {
        StartCoroutine(SceneLoader.LoadSceneQuick
        (
            loadScene, 
            onComplete
        ));
    }

    void OnApplicationQuit()
    {
        SaveSystem.Save();
    }
}

public enum GameScene
{
    Loading = 0,
    Home = 1,
    Gameplay = 2,
}