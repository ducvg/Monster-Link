using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GameBoard
{
    private static GameTile[,] board;
    private static int colLength = 0;
    private static int rowLength = 0;

    public static void OnInit()
    {
        board = BoardManager.Instance.board;
        colLength = BoardManager.Instance.BoardSize.y;
        rowLength = BoardManager.Instance.BoardSize.x;
    }

    public static List<(int x, int y)> TurnBFS((int x, int y) startPos, (int x, int y) target)
    {
        // BFS queue: queue positions to visit with its reached dir and turns taken to reach it.
        Queue<((int x, int y) pos, (int dx, int dy) reachDir, int turnsTaken)> frontier = new();
        // Dictionary to track visited positions of a direction. Return turns taken to reach it.
        Dictionary<((int x, int y) pos, (int dx, int dy) reachDir), int> visited = new();
        // Backtrack dictionary storing position with direction to reach it. Returns previous position and direction to reach that previous position.
        Dictionary<((int x, int y) pos, (int dx, int dy) reachDir), ((int x, int y) prevPos, (int dx, int dy) prevReachDir)> cameFrom = new();

        frontier.Enqueue((startPos, (0, 0), -1)); //enqueue startPos, (0,0) as no direction, -1 turns taken to nulify the no direction

        while (frontier.Count > 0)
        {
            var (currentPos, currentDir, currentTurns) = frontier.Dequeue();

            if (currentPos == target)
            {
                List<(int x, int y)> path = new();

                // Start backtracking from target
                ((int x, int y) pos, (int x, int y) reachDir) parent = (currentPos, currentDir);
                while (cameFrom.ContainsKey(parent))
                {
                    path.Add(parent.pos);
                    parent = cameFrom[parent];
                }
                path.Add(startPos);
                path.Reverse();

                return path;
            }

            foreach (var nextDir in Direction.NoDiagonal)
            {
                var nextPos = (currentPos.x + nextDir.x, currentPos.y + nextDir.y);

                if (IsBlocked(nextPos) && nextPos != target)
                    continue;

                int nextTurns = (currentDir != nextDir) ? currentTurns + 1 : currentTurns;

                if (nextTurns > 2)
                    continue;

                var next = (nextPos, nextDir);
                if (visited.ContainsKey(next) && visited[next] <= nextTurns)
                    continue;

                frontier.Enqueue((nextPos, nextDir, nextTurns));
                visited[next] = nextTurns;
                cameFrom[next] = (currentPos, currentDir);
            }
        }

        return null;
    }

    public static List<(int x, int)> Connect(MatchTile start, MatchTile target)
        => TurnBFS((start.BoardPosition.x, start.BoardPosition.y), (target.BoardPosition.x, target.BoardPosition.y));


    private static bool IsBlocked(int x, int y) => IsBlocked((x, y));
    private static bool IsBlocked((int x, int y) position)
    {
        if (position.x < -1 || position.x > rowLength
        || position.y < -1 || position.y > colLength) //out of bounds
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

    public static List<(int x, int)> FindAnyPath()
    {
        Dictionary<MatchTileData, List<MatchTile>> tileGroups = new();

        for (int x = 0; x < rowLength; x++)
        {
            for (int y = 0; y < colLength; y++)
            {
                var tile = board[x, y];
                if (tile is MatchTile matchTile)
                {
                    if (!tileGroups.ContainsKey(matchTile.matchTileData))
                    {
                        tileGroups[matchTile.matchTileData] = new List<MatchTile>();
                    }
                    tileGroups[matchTile.matchTileData].Add(matchTile);
                }
            }
        }

        foreach (var group in tileGroups.Keys)
        {
            for (int i = 0; i < tileGroups[group].Count; i++)
            {
                for (int j = i + 1; j < tileGroups[group].Count; j++)
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

    public static void Shuffle()
    {
        List<MatchTile> matchTiles = GetMatchTiles();

        while (matchTiles.Count >= 2)
        {
            var tile1 = matchTiles.GetRandomElement();
            matchTiles.Remove(tile1);
            var tile2 = matchTiles.GetRandomElement();
            matchTiles.Remove(tile2);

            (board[tile1.BoardPosition.x, tile1.BoardPosition.y], board[tile2.BoardPosition.x, tile2.BoardPosition.y]) = (tile2, tile1);

            var tmp = tile1.transform.position;
            tile1.transform.position = tile2.transform.position;
            tile2.transform.position = tmp;

            var tmpBoardPos = tile1.BoardPosition;
            tile1.BoardPosition = tile2.BoardPosition;
            tile2.BoardPosition = tmpBoardPos;
        }

        if (FindAnyPath() == null)
        {
            Debug.LogWarning("No path found after shuffling.");
            Shuffle();
        }
    }

    public static List<MatchTile> GetMatchTiles()
    {
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

    public static bool IsMatchTile(this GameTile tile)
    {
        return tile != null && tile is MatchTile;
    }

}


public static class Direction
{
    public static readonly List<(int x, int y)> NoDiagonal = new()
    {
        (1, 0), // Right
        (-1, 0), // Left
        (0, 1), // Up
        (0, -1) // Down
    };
}