using UnityEngine;

public class ObstacleTile : GameTile
{
    protected override void Awake()
    {
        base.Awake();
        // Additional initialization for obstacle tiles can be added here
    }

    public override void OnInit()
    {
        base.OnInit();
        // Custom initialization logic for obstacle tiles
    }

    public override void OnDespawn()
    {
        // Custom cleanup logic for obstacle tiles
        base.OnDespawn();
    }
}
