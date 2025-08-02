using UnityEngine;

public class GameplayCanvasStarter : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.CloseImmediate<GameplayCanvas>();
        UIManager.Instance.Open<GameplayCanvas>().OnInit();
        Destroy(gameObject);
    }
}
