using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerDataProfileSO playerDataProfile;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        SaveSystem.Load();
        playerDataProfile.Load();
    }

    void Start()
    {
        LoadScene(GameScene.Home,
        onComplete:() =>
        {
            UIManager.Instance.Open<HomeCanvas>();
            LevelManager.Instance.OnInit();
            SoundManager.Instance.ChangeSound(SoundID.Home_BG, 1f);
        }
        );   
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