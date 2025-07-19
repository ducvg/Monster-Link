using UnityEngine;

public abstract class ObstacleTileFactory : ScriptableObject
{
    public abstract ObstacleTile CreateObstacleTile(ObstacleTile obstacleTilePrefab, Vector3 position);
}
