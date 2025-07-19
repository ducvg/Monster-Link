using UnityEngine;

public class WaterTile : ObstacleTile
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        Destroy(spriteRenderer);
    }
}
