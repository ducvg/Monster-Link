using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : BaseCanvas
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Animator animator;

    public float LoadingBar { get => loadingBar.fillAmount; set => loadingBar.fillAmount = value; }
    public string ProgressText 
    { 
        get => progressText.text; 
        set
        {
            StringBuilder sb = new();
            sb.Append(value);
            sb.Append("%");
            progressText.text = sb.ToString();
        } 
    }

    public void OnInit()
    {
        loadingBar.fillAmount = 0;
        progressText.text = "0%";
        loadingText.text = "Loading...";
        animator.SetTrigger(AnimationHash.OnReset);
    }

    public void OnComplete()
    {
        loadingBar.fillAmount = 1;
        progressText.text = "100%";
        loadingText.text = "Completed !";
        animator.SetTrigger(AnimationHash.OnComplete);
    }
}
