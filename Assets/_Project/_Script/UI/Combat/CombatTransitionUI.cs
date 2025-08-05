using UnityEngine;
using Utility.SkibidiTween;

public class CombatTransitionUI : MonoBehaviour
{
    [SerializeField] private Skibidi<float> fadeSettings;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private SkibidiElement[] skibidiElements;

    private void Awake()
    {
        skibidiElements = GetComponentsInChildren<SkibidiElement>();
    }

    [ContextMenu("Show")]
    public void Show()
    {
        gameObject.SetActive(true);

        foreach (var element in skibidiElements)
        {
            element.Show();
        }
        StartCoroutine(canvasGroup.SkibidiFade(fadeSettings, onComplete: null, isReverse: true));
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        foreach (var element in skibidiElements)
        {
            element.Hide();
        }
        StartCoroutine(canvasGroup.SkibidiFade(fadeSettings, 
        onComplete: () => gameObject.SetActive(false), 
        isReverse: false));
    }
}
