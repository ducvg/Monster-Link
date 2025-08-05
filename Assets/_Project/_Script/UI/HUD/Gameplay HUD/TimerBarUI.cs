using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerBarUI : MonoBehaviour
{
    [SerializeField, InspectorName("Remain time")] private float timer = 10f;
    [SerializeField] private Gradient timerGradient;
    [SerializeField] private Image timerFill;
    [SerializeField] private TextMeshProUGUI timerText;


    private float maxTimeSeconds;
    private bool isTimerRunning = true;
    private float warnTime = 0f;
    private event Action warnTimeAction;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {        
        maxTimeSeconds = BoardManager.Instance.LevelTime;
    
        timer = maxTimeSeconds;
        warnTime = maxTimeSeconds * 0.25f;
        isTimerRunning = true;

        UpdateUI();
        warnTimeAction = WarnTime;
    }

    void Update()
    {
        if (!isTimerRunning) return;
        
        timer -= Time.deltaTime;
        timerFill.color = timerGradient.Evaluate(timer / maxTimeSeconds);
        
        if (timer <= 0)
        {
            timer = 0;
            isTimerRunning = false;
            GameState.OnGameLost?.Invoke();
            UIManager.Instance.Open<GameplayLoseCanvas>();
        }

        warnTimeAction?.Invoke();
        UpdateUI();
    }

    private void WarnTime()
    {
        if(timer > warnTime) return;

        SoundManager.Instance.PlayFx(FxID.Time_Warning);
        warnTimeAction = null;
    }

    private void UpdateUI()
    {
        timerFill.fillAmount = timer / maxTimeSeconds;
        TimeSpan time = TimeSpan.FromSeconds(timer);
        timerText.text = $"{time.Minutes:00}:{time.Seconds:00}";
    }

    private void PauseTimer()
    {
        isTimerRunning = false;
    }

    private void ResumeTimer()
    {
        isTimerRunning = true;
    }

    void OnEnable()
    {
        GameState.OnGameWon += PauseTimer;
        GameState.OnGamePause += PauseTimer;
        GameState.OnGameResume += ResumeTimer;
    }

    void OnDisable()
    {
        GameState.OnGameWon -= PauseTimer;
        GameState.OnGamePause -= PauseTimer;
        GameState.OnGameResume -= ResumeTimer;
    }

}
