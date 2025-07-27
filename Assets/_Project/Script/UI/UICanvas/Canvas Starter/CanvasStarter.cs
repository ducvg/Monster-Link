using UnityEngine;

public abstract class CanvasStarter<T> : MonoBehaviour where T : BaseCanvas
{
    protected virtual void Start()
    {
        UIManager.Instance.Open<T>();
        Destroy(this);
    }   
}

