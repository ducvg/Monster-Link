using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility.SkibidiTween;

public class SkibidiElement : MonoBehaviour
{
    [SerializeField] private Skibidi<Vector2> moveSettings;
    [SerializeField] private Skibidi<Vector3> rotationSettings;
    [SerializeField] private Skibidi<Vector3> scaleSettings;
    [SerializeField] private Skibidi<Color> colorSettings;

    [SerializeField] private Graphic elementGraphic;

    [SerializeField] private UnityEvent onShow, onHide;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    public void Show()
    {
        StopAllCoroutines();

        onShow?.Invoke();
    }

    public void Move(bool isReverse) => StartCoroutine(rectTransform.SkibidiMove(moveSettings, null, isReverse));
    public void Rotate(bool isReverse) => StartCoroutine(rectTransform.SkibidiRotate(rotationSettings, null, isReverse));
    public void Scale(bool isReverse) => StartCoroutine(rectTransform.SkibidiScale(scaleSettings, null, isReverse));
    public void Fade(bool isReverse) => StartCoroutine(elementGraphic.SkibidiFade(colorSettings, null, isReverse));

    public void RotateLoop(bool isPingPong) => StartCoroutine(rectTransform.SkibidiRotateLoop(rotationSettings, isPingPong));
    public void ScaleLoop(bool isPingPong) => StartCoroutine(rectTransform.SkibidiScaleLoop(scaleSettings, isPingPong));

    public void Hide()
    {
        StopAllCoroutines();

        onHide?.Invoke();
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
