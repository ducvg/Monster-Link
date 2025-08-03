using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static IEnumerator LoadScene(GameScene loadScene, Action onComplete, Task additionalTask = null)
    {
        UIManager.Instance.CloseAll();
        LoadingCanvas loadingCanvas = UIManager.Instance.Open<LoadingCanvas>();
        loadingCanvas.OnInit();
    
        AsyncOperation scene = SceneManager.LoadSceneAsync((int)loadScene);
        scene.allowSceneActivation = false;

        while (loadingCanvas.LoadingBar < 0.9f) // stop at 0.9f until allowSceneActivation is true
        {
            loadingCanvas.LoadingBar = Mathf.MoveTowards(loadingCanvas.LoadingBar, scene.progress, Time.deltaTime);
            loadingCanvas.ProgressText = (loadingCanvas.LoadingBar * 100).ToString("F0");
            yield return null;
        }
        loadingCanvas.OnComplete();
        while(loadingCanvas.LoadingBar < 1f)
        {
            loadingCanvas.LoadingBar = Mathf.MoveTowards(loadingCanvas.LoadingBar, 1f, Time.deltaTime);
            loadingCanvas.ProgressText = (loadingCanvas.LoadingBar * 100).ToString("F0");
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        UIManager.Instance.Close<LoadingCanvas>();

        var transitionCanvas = UIManager.Instance.Open<CrossFadeCanvas>();
        
        transitionCanvas.OpenCompleteAction += () =>
        {
            scene.allowSceneActivation = true;
            transitionCanvas.OpenCompleteAction = null;
        };

        yield return scene;
        UIManager.Instance.Close<CrossFadeCanvas>();
        onComplete?.Invoke();

    }

    public static IEnumerator LoadSceneQuick(GameScene loadScene, Action onComplete = null)
    {
        UIManager.Instance.CloseAll();
        var transitionCanvas = UIManager.Instance.Open<CrossFadeCanvas>();

        var scene = SceneManager.LoadSceneAsync((int)loadScene);
        scene.allowSceneActivation = false;
        
        transitionCanvas.OpenCompleteAction += () =>
        {
            scene.allowSceneActivation = true;
            transitionCanvas.OpenCompleteAction = null;
        };
        yield return scene;
        UIManager.Instance.Close<CrossFadeCanvas>();
        onComplete?.Invoke();
    }
}
