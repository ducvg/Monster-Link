using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TimerBarUI : MonoBehaviour
{
    [SerializeField] private Image timerFill;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float maxTimeSeconds = 600f;

    private float timer;
    private bool isTimerRunning = true;

    public void OnInit()
    {        
        timer = maxTimeSeconds;
        isTimerRunning = true;
        UpdateUI();
    }

    void Update()
    {
        if (!isTimerRunning) return;
        
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            timer = 0;
            isTimerRunning = false;
            GameState.OnGameLost?.Invoke();
            UIManager.Instance.Open<GameplayLoseCanvas>();
        }

        UpdateUI();
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
        GameState.OnGamePause += PauseTimer;
        GameState.OnGameResume += ResumeTimer;
    }

    void OnDisable()
    {
        GameState.OnGamePause -= PauseTimer;
        GameState.OnGameResume -= ResumeTimer;
    }

}
