using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : PersistentSingleton<GameManager>
{

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

    public IEnumerator LoadScene(string sceneName, Action onComplete, float delayScene = 1f)
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
        } while (scene.progress < 0.9f); // stop at 0.9f until allowSceneActivation is true

        while(loadingCanvas.LoadingBar < 1f)
        {
            loadingCanvas.LoadingBar = Mathf.MoveTowards(loadingCanvas.LoadingBar, 1f, Time.deltaTime);
            loadingCanvas.ProgressText = (loadingCanvas.LoadingBar * 100).ToString("F0");
            yield return null;
        }
        loadingCanvas.OnComplete();
        
        yield return new WaitForSeconds(delayScene);
        scene.allowSceneActivation = true;

        yield return null;
        UIManager.Instance.Close<LoadingCanvas>();
        onComplete?.Invoke();



    }


}


public static class GameScene
{
    public const string Home = "Home";
    public const string Gameplay = "Gameplay";
    public const string Loading = "Loading";
}