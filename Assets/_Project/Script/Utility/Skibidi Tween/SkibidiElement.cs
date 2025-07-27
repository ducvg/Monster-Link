using System;
using UnityEngine;
using Utility.SkibidiTween;

public class SkibidiElement : MonoBehaviour
{
    [SerializeField] private Skibidi<Vector2> moveSettings;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    public void Show()
    {
        StopAllCoroutines();

        StartCoroutine(rectTransform.SkibidiMove(moveSettings, null, isReverse: true));
    }

    public void Hide()
    {
        StopAllCoroutines();

        StartCoroutine(rectTransform.SkibidiMove(moveSettings, null, isReverse: false));
    }

#region Debug
    [ContextMenu("Set as Show")]
    public void SetAsShow()
    {
        (transform as RectTransform).anchoredPosition3D = moveSettings.start;
    }

    [ContextMenu("Set as Hide")]
    public void SetAsHide()
    {
        (transform as RectTransform).anchoredPosition3D = moveSettings.end;
    }
    
#endregion
}
