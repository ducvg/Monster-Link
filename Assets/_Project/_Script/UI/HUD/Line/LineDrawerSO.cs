using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line Drawer", menuName = "Scriptable Objects/Line/Line Drawer")]
public class LineDrawerSO : ScriptableObject
{
    [SerializeField] private LineFactorySO lineFactory;

    public void DrawLine(List<(int x, int y)> path, float duration = 0f)
    {
        Vector3[] pathVector = new Vector3[path.Count];

        int count = path.Count;
        var tilemap = BoardManager.Instance.BoardTilemap;
        for (int i = 0; i < count; i++)
        {
            Vector3Int tilePosition = new(path[i].x, path[i].y, 0);
            pathVector[i] = tilemap.GetCellCenterWorld(tilePosition);
        }

        var line = lineFactory.CreateLine(pathVector);
        var matchTile = BoardManager.Instance.board[path[0].x, path[0].y] as MatchTile;
        matchTile.lineDespawnAction += () =>
        {
            line.OnDespawn(duration);
            matchTile.lineDespawnAction = null;
        };
    }
}
