using UnityEngine;

public abstract class CanvasStarter<T> : MonoBehaviour where T : BaseCanvas
{
    [ContextMenu("Open")]
    protected virtual void Start()
    {
        UIManager.Instance.Open<T>();
    }   

    [ContextMenu("Close")]
    protected virtual void Close()
    {
        UIManager.Instance.Close<T>();
    }
}

