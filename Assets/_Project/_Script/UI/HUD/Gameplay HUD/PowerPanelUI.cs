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
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        SoundManager.Instance.PlayFx(FxID.Button);
        
        if(gameObject.activeSelf) Hide();
        else Show();
    }

    public void Show()
    {
        if (isSkibiding) return;
        gameObject.SetActive(true);
        isSkibiding = true;

        StartCoroutine(rectTransform.SkibidiMove(
            moveSettings, 
            () =>
            {
                isSkibiding = false;
            }, 
            true
        ));
    }

    public void Hide()
    {
        if (isSkibiding) return;
        isSkibiding = true;

        StartCoroutine(rectTransform.SkibidiMove(
            moveSettings, 
            () =>
            {
                isSkibiding = false;
                gameObject.SetActive(false);
            }, 
            false
        ));
    }
}
