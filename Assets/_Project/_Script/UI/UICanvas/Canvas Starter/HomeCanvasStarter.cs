using UnityEngine;

public class HomeCanvasStarter : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.CloseImmediate<HomeCanvas>();
        UIManager.Instance.Open<HomeCanvas>();
        Destroy(gameObject);
    }
}
