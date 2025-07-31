using UnityEngine;
using Utility.SkibidiTween;

public class PowerPanelUI : MonoBehaviour
{
    [SerializeField] private Skibidi<Vector2> moveSettings;

    private RectTransform rectTransform;
    private bool isSkibiding = false;
    
    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    public void Show()
    {
        if (isSkibiding) return;
        isSkibiding = true;
        StartCoroutine(rectTransform.SkibidiMove(moveSettings, () => isSkibiding = false, true));
    }

    public void Hide()
    {
        if (isSkibiding) return;
        isSkibiding = true;
        StartCoroutine(rectTransform.SkibidiMove(moveSettings, () => isSkibiding = false, false));
    }
}
