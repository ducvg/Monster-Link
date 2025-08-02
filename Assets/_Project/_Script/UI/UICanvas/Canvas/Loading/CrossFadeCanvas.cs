using System;
using UnityEngine;

public class CrossFadeCanvas : BaseCanvas
{
    [Header("Cross Fade Canvas Settings")]
    public Action OpenCompleteAction, CloseCompleteAction;

    public override void OnOpenComplete()
    {
        base.OnOpenComplete();
        OpenCompleteAction?.Invoke();
    }

    public override void OnCloseComplete()  
    {
        base.OnCloseComplete();
        CloseCompleteAction?.Invoke();
    }    
}
