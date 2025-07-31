using UnityEngine;
using UnityEngine.Pool;

public class MonsterTile : MatchTile
{
    public override void OnInit()
    {
        base.OnInit();
        // Additional initialization for MonsterTile if needed
    }

    protected override void Start()
    {
        base.Start();
        // Additional start logic for MonsterTile if needed
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        
    }

}
