using UnityEngine;
using UnityEngine.UI;
using Utility.SkibidiTween;

public class LoadingTitle : MonoBehaviour
{
    [SerializeField] private Skibidi<Vector2> moveSetting;
    [SerializeField] private Skibidi<Color> colorSetting;
    [SerializeField] private Graphic titleText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine((transform as RectTransform).SkibidiMove(moveSetting, null, isReverse: true));
        StartCoroutine(titleText.SkibidiColor(colorSetting, null, isReverse: true));
    }

}
