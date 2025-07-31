using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public static class GameBoard
{
    public static void Shuffle()
    {
        List<MatchTile> matchTiles = GetMatchTiles();

        if(matchTiles.Count == 0) return;

        do
        {
            int rolls = (int)Random.Range(matchTiles.Count * 0.5f, matchTiles.Count);
            while (rolls-- > 0)
            {
                var tile1 = matchTiles.GetRandomElement();
                MatchTile tile2;
                do
                {
                    tile2 = matchTiles.GetRandomElement();
                } while (tile1 == tile2);

                SwapTiles(tile1, tile2);
            }
        } while (FindAnyPath() == null);
    }

    private static void SwapTiles(GameTile tile1, GameTile tile2)
    {
        var board = BoardManager.Instance.board;

        (board[tile1.BoardPosition.x, tile1.BoardPosition.y], board[tile2.BoardPosition.x, tile2.BoardPosition.y]) = (tile2, tile1);
        (tile2.transform.position, tile1.transform.position) = (tile1.transform.position, tile2.transform.position);
        (tile2.BoardPosition, tile1.BoardPosition) = (tile1.BoardPosition, tile2.BoardPosition);
    }

#region Connect 2 tiles
    public static List<(int x, int y)> TurnBFS((int x, int y) start, (int x, int y) target)
    {
        List<(int x, int y)> searchDirection = new(){(1, 0), (-1, 0), (0, 1), (0, -1)};

        Queue<((int x, int y) pos, (int dx, int dy) reachDir, int turnsTaken)> frontier = new();
        //return turns taken to reach this visited position
        Dictionary<((int x, int y) pos, (int dx, int dy) turnsTaken), int> visited = new();
        //return prev position with direction to reach it
        Dictionary<((int x, int y) pos, (int dx, int dy) reachDir), ((int x, int y) prevPos, (int dx, int dy) prevReachDir)> cameFrom = new();

        frontier.Enqueue((start, (0, 0), -1)); //enqueue start, (0,0): no direction, -1 turn taken: nulify the no direction turn

        while (frontier.Count > 0)
        {
            var (currentPos, currentDir, currentTurns) = frontier.Dequeue();

            if (currentPos == target)
            {
                List<(int x, int y)> path = new();

                ((int x, int y) pos, (int x, int y) reachDir) parent = (currentPos, currentDir);
                while (cameFrom.ContainsKey(parent))
                {
                    path.Add(parent.pos);
                    parent = cameFrom[parent];
                }
                path.Add(start);
                // path.Reverse();

                return path;
            }

            foreach (var nextDir in searchDirection)
            {
                if(nextDir == (-currentDir.dx, -currentDir.dy)) continue;
                
                var nextPos = (currentPos.x + nextDir.x, currentPos.y + nextDir.y);

                if (IsBlocked(nextPos) && nextPos != target) continue;

                int nextTurns = (currentDir != nextDir) ? currentTurns + 1 : currentTurns;
                if (nextTurns > 2) continue;

                var next = (nextPos, nextDir);
                if (visited.ContainsKey(next) && visited[next] <= nextTurns) continue;

                frontier.Enqueue((nextPos, nextDir, nextTurns));
                visited[next] = nextTurns;
                cameFrom[next] = (currentPos, currentDir);
            }
        }

        return null;
    }

    public static List<(int x, int)> Connect(MatchTile start, MatchTile target)
        => TurnBFS((start.BoardPosition.x, start.BoardPosition.y), (target.BoardPosition.x, target.BoardPosition.y));

    private static bool IsBlocked((int x, int y) position)
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var rowLength = BoardManager.Instance.BoardSize.x;

        if (position.x < -1 || position.x > rowLength
        || position.y < -1 || position.y > colLength) //out of bounds cases
        {
            return true;
        }

        if (position.x == -1 || position.x == rowLength
        || position.y == -1 || position.y == colLength) //allow outside 1 for edge searches
        {
            return false;
        }

        var tile = board[position.x, position.y];
        return tile != null && tile.IsBlockable;
    }
#endregion

#region Find any connection
    public static List<(int x, int y)> FindAnyPath()
    {
        Dictionary<MatchTileData, List<MatchTile>> tileGroups = FilterTileGroups();

        foreach (var group in tileGroups.Keys)
        {
            int count = tileGroups[group].Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    var startTile = tileGroups[group][i];
                    var targetTile = tileGroups[group][j];
                    
                    var path = Connect(startTile, targetTile);
                    if (path != null)
                    {
                        return path;
                    }
                }
            }
        }
        return null;
    }

    private static Dictionary<MatchTileData, List<MatchTile>> FilterTileGroups()
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var rowLength = BoardManager.Instance.BoardSize.x;

        Dictionary<MatchTileData, List<MatchTile>> tileGroups = new(); //new list for each tile type

        for (int x = 0; x < rowLength; x++)
        {
            for (int y = 0; y < colLength; y++)
            {
                var tile = board[x, y];
                if (tile is MatchTile matchTile)
                {
                    if (!tileGroups.ContainsKey(matchTile.MatchTileData))
                    {
                        tileGroups[matchTile.MatchTileData] = new List<MatchTile>();
                    }
                    tileGroups[matchTile.MatchTileData].Add(matchTile);
                }
            }
        }

        return tileGroups;
    }
#endregion

    public static List<(int x, int y)> GetEmptyPositions()
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var rowLength = BoardManager.Instance.BoardSize.x;

        List<(int x, int y)> unfilledPositions = new();
        for (int x = 0; x < rowLength; x++)
        {
            for (int y = 0; y < colLength; y++)
            {
                if (board[x, y] == null)
                {
                    unfilledPositions.Add((x, y));
                }
            }
        }
        return unfilledPositions;
    }

    public static List<MatchTile> GetMatchTiles()
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var rowLength = BoardManager.Instance.BoardSize.x;

        List<MatchTile> matchTiles = new();

        for (int x = 0; x < rowLength; x++)
        {
            for (int y = 0; y < colLength; y++)
            {
                var tile = board[x, y];
                if (tile is MatchTile matchTile)
                {
                    matchTiles.Add(matchTile);
                }
            }
        }

        return matchTiles;
    }
    
#region Apply Gravity
    public static void ApplyGravity(GameTile tileConnected, GravityDirection gravityDirection)
    {
        switch (gravityDirection)
        {
            case GravityDirection.Up:
                ApplyGravityUp(tileConnected);
                break;
            case GravityDirection.Down:
                ApplyGravityDown(tileConnected);
                break;
            case GravityDirection.Left:
                ApplyGravityLeft(tileConnected);
                break;
            case GravityDirection.Right:
                ApplyGravityRight(tileConnected);
                break;
        }
    }

    private static void ApplyGravityUp(GameTile tileConnected) //pull all tiles up this in this tile column
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var tilemap = BoardManager.Instance.BoardTilemap;
        int x = tileConnected.BoardPosition.x;

        for (int y = colLength - 1; y > 0; y--)
        {
            GameTile upperTile = board[x, y];
            if(upperTile != null) continue;
            GameTile tileToPull = board[x, y-1];
            if(tileToPull == null || !tileToPull.IsMovable) continue;

            tileToPull.MoveTo(tilemap.GetCellCenterWorld(new Vector3Int(x, y)));
            tileToPull.BoardPosition = new Vector3Int(x, y);
            board[x, y] = tileToPull;
            board[x, y-1] = null;
        }
        
    }

    private static void ApplyGravityDown(GameTile tileConnected)
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var tilemap = BoardManager.Instance.BoardTilemap;
        
        int x = tileConnected.BoardPosition.x;
        for (int y = 0; y < colLength - 1; y++)
        {
            GameTile lowerTile = board[x, y];
            if(lowerTile != null) continue;
            GameTile tileToPush = board[x, y+1];
            if(tileToPush == null || !tileToPush.IsMovable) continue;

            tileToPush.MoveTo(tilemap.GetCellCenterWorld(new Vector3Int(x, y)));
            tileToPush.BoardPosition = new Vector3Int(x, y);
            board[x, y] = tileToPush;
            board[x, y+1] = null;
        }
    }

    private static void ApplyGravityLeft(GameTile tileConnected)
    {
        var board = BoardManager.Instance.board;
        var rowLength = BoardManager.Instance.BoardSize.x;
        var tilemap = BoardManager.Instance.BoardTilemap;

        int y = tileConnected.BoardPosition.y;
        for (int x = 0; x < rowLength - 1; x++)
        {
            GameTile leftTile = board[x, y];
            if(leftTile != null) continue;
            GameTile tileToPush = board[x+1, y];
            if(tileToPush == null || !tileToPush.IsMovable) continue;

            tileToPush.MoveTo(tilemap.GetCellCenterWorld(new Vector3Int(x, y)));
            tileToPush.BoardPosition = new Vector3Int(x, y);
            board[x, y] = tileToPush;
            board[x+1, y] = null;
        }
    }   

    private static void ApplyGravityRight(GameTile tileConnected)
    {
        var board = BoardManager.Instance.board;
        var rowLength = BoardManager.Instance.BoardSize.x;
        var tilemap = BoardManager.Instance.BoardTilemap;

        int y = tileConnected.BoardPosition.y;
        for (int x = rowLength - 1; x > 0; x--)
        {
            GameTile rightTile = board[x, y];
            if(rightTile != null) continue;
            GameTile tileToPush = board[x-1, y];
            if(tileToPush == null || !tileToPush.IsMovable) continue;

            tileToPush.MoveTo(tilemap.GetCellCenterWorld(new Vector3Int(x, y)));
            tileToPush.BoardPosition = new Vector3Int(x, y);
            board[x, y] = tileToPush;
            board[x-1, y] = null;
        }
    }
#endregion

}
