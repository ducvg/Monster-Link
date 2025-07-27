using System.Collections.Generic;
using UnityEngine;
using Utility.SkibidiTween;

[RequireComponent(typeof(CanvasGroup))]
public class BaseCanvas : MonoBehaviour
{
    [Header("Base Canvas Settings")]
    [SerializeField] protected Skibidi<float> fadeSettings;
    [SerializeField] protected CanvasGroup canvasGroup;

    protected SkibidiElement[] elementSettings;

    protected bool isTweening = false; //track if canvas is fully open or closed

    void Awake()
    {
        elementSettings = GetComponentsInChildren<SkibidiElement>(includeInactive: true);
    }

    public virtual void Setup()
    {        
        gameObject.SetActive(true);

        Open();
    }

    protected virtual void Open()
    {
        if(isTweening) return;
        isTweening = true;
        
        canvasGroup.interactable = false;

        for(int i = 0; i < elementSettings.Length; i++)
        {
            elementSettings[i].Show();
        }

        StartCoroutine(canvasGroup.SkibidiFade(fadeSettings, OnOpenComplete, isReverse: true));
    }

    protected virtual void OnOpenComplete()
    {
        isTweening = false;
        canvasGroup.interactable = true;
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
        
        canvasGroup.interactable = false;

        for(int i = 0; i < elementSettings.Length; i++)
        {
            elementSettings[i].Hide();
        }
        StartCoroutine(canvasGroup.SkibidiFade(fadeSettings, OnCloseComplete, isReverse: false));
    }

    protected virtual void OnCloseComplete()
    {
        isTweening = false;
        canvasGroup.interactable = true;
        gameObject.SetActive(false);
    }

#region Editor
    [ContextMenu("Set as Show")]
    public void EditorSetAsShow()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        elementSettings = GetComponentsInChildren<SkibidiElement>(includeInactive: true);

        canvasGroup.alpha = fadeSettings.start;

        for(int i = 0; i < elementSettings.Length; i++)
        {
            elementSettings[i].SetAsShow();
        }
    }

    [ContextMenu("Set as Hidden")]
    public void EditorSetAsHidden()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        elementSettings = GetComponentsInChildren<SkibidiElement>(includeInactive: true);

        canvasGroup.alpha = fadeSettings.end;

        for(int i = 0; i < elementSettings.Length; i++)
        {
            elementSettings[i].SetAsHide();
        }
    }
#endregion
}
