using UnityEngine;
using Utility.SkibidiTween;

[RequireComponent(typeof(CanvasGroup))]
public class BaseCanvas : MonoBehaviour
{
    [SerializeField] protected Skibidi<Vector3> moveSettings;
    [SerializeField] protected Skibidi<float> fadeSettings;

    [SerializeField] private CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    protected bool isTweening = false; //track if canvas is fully open or closed

    protected virtual void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    public virtual void Setup()
    {
        if (isTweening) return;
        isTweening = true;
        gameObject.SetActive(true);

        Open();
    }

    protected virtual void Open()
    {
        StartCoroutine(rectTransform.SkibidiMove(moveSettings, OnOpenComplete));
        StartCoroutine(canvasGroup.SkibidiFadeCanvas(fadeSettings, OnOpenComplete));
    }

    protected virtual void OnOpenComplete()
    {
        isTweening = false;
    }

    public virtual void Close(float delay)
    {
        if (isTweening) return;
        isTweening = true;

        StartCoroutine(SkibidiTween.SkibidiDelay(delay, onComplete: Close));
    }

    public virtual void Close()
    {
        if (isTweening) return;
        isTweening = true;

        StartCoroutine(rectTransform.SkibidiMove(moveSettings, OnCloseComplete, isReverse: true));
        StartCoroutine(canvasGroup.SkibidiFadeCanvas(fadeSettings, OnCloseComplete, isReverse: true));
    }

    protected virtual void OnCloseComplete()
    {
        isTweening = false;
        gameObject.SetActive(false);
    }

#region Editor
    [ContextMenu("Set as Show")]
    public void EditorSetAsShow()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition3D = moveSettings.start;
    }

    [ContextMenu("Set as Hidden")]
    public void EditorSetAsHidden()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        rectTransform.anchoredPosition3D = moveSettings.end;
    }
#endregion
}
