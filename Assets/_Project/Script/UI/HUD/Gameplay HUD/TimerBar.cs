using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
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
        UpdateUI();
        
        if (timer <= 0)
        {
            isTimerRunning = false;
            GameState.OnGameLost?.Invoke();
        }
    }

    private void UpdateUI()
    {
        timerFill.fillAmount = timer / maxTimeSeconds;
        timerText.text = $"{Mathf.FloorToInt(timer / 60):00} : {Mathf.FloorToInt(timer % 60):00}";
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
