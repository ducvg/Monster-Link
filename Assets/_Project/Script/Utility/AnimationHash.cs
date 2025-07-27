using UnityEngine;

public static class AnimationHash
{
    public static readonly int OnSelect = Animator.StringToHash("OnSelect");
    public static readonly int OnDeselect = Animator.StringToHash("OnDeselect");
    public static readonly int OnConnect = Animator.StringToHash("OnConnect");

    public static readonly int OnComplete = Animator.StringToHash("OnComplete");
    public static readonly int OnReset = Animator.StringToHash("OnReset");
}   