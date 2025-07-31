using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private float delaySceneTime = 1f;

    void Start()
    {
        StartCoroutine(LoadScene
        (
            GameScene.Gameplay, 
            onComplete: () => 
            {
                UIManager.Instance.Open<GameplayCanvas>().OnInit();
            }
        ));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            UIManager.Instance.Open<GameplayWinCanvas>();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            UIManager.Instance.Open<GameplayLoseCanvas>();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.CloseAll();
        }
    }

    public IEnumerator LoadScene(string sceneName, Action onComplete)
    {
        UIManager.Instance.CloseAll();
        var loadingCanvas = UIManager.Instance.Open<LoadingCanvas>();
        loadingCanvas.OnInit();
    
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            loadingCanvas.LoadingBar = Mathf.MoveTowards(loadingCanvas.LoadingBar, scene.progress, Time.deltaTime);
            loadingCanvas.ProgressText = (loadingCanvas.LoadingBar * 100).ToString("F0");
            yield return null;
        } while (loadingCanvas.LoadingBar < 0.9f); // stop at 0.9f until allowSceneActivation is true

        loadingCanvas.OnComplete();
        while(loadingCanvas.LoadingBar < 1f)
        {
            loadingCanvas.LoadingBar = Mathf.MoveTowards(loadingCanvas.LoadingBar, 1f, Time.deltaTime);
            loadingCanvas.ProgressText = (loadingCanvas.LoadingBar * 100).ToString("F0");
            yield return null;
        }
        
        yield return new WaitForSeconds(delaySceneTime);
        scene.allowSceneActivation = true;
        yield return null;
        UIManager.Instance.Close<LoadingCanvas>();
        onComplete?.Invoke();
    }

    private void GameWin()
    {
        UIManager.Instance.Open<GameplayWinCanvas>();
    }
    private void GameLose()
    {
        UIManager.Instance.Open<GameplayLoseCanvas>();
    }

    private void PauseGame() 
    {
        // GameState.isPause = true;
    }
    private void ResumeGame() 
    {
        // GameState.isPause = false;
    }

    void OnEnable()
    {
        GameState.OnGamePause += PauseGame;
        GameState.OnGameResume += ResumeGame;
        GameState.OnGameWon += GameWin;
        GameState.OnGameLost += GameLose;
    }
    
    void OnDisable()
    {
        GameState.OnGamePause -= PauseGame;
        GameState.OnGameResume -= ResumeGame;
        GameState.OnGameWon -= GameWin;
        GameState.OnGameLost -= GameLose;
    }

}


public static class GameScene
{
    public const string Home = "Home";
    public const string Gameplay = "Gameplay";
    public const string Loading = "Loading";
}