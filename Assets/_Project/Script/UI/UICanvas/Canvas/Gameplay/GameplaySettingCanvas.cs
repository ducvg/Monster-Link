using UnityEngine;
using Utility.SkibidiTween;

public class GameplaySettingCanvas : BaseCanvas
{

    public void OnResumeClick()
    {
        UIManager.Instance.Close<GameplaySettingCanvas>();

        GameState.OnGameResume?.Invoke();
    }

    public void OnQuitClick()
    {
        UIManager.Instance.Close<GameplaySettingCanvas>();
        
        GameState.OnGameResume?.Invoke();
        GameState.OnGameLost?.Invoke();
    }
}
