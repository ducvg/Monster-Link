using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class TileConnection
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
        Dictionary<(int x, int y), (int x, int y)> cameFrom = new(); // Track tile comes from which tiles

        Queue<((int x, int y) pos, (int dx, int dy)? dir, int turns)> frontier = new(); //frontier queue hold pos to visit, direction reach it and turns at this position
        HashSet<((int x, int y), (int dx, int dy)?)> visited = new(); // visited set hold unique positions and direction to reach it

        frontier.Enqueue((startPos, null, 0)); //start position have no initial direction and 0 turns
        cameFrom[startPos] = startPos; // Start has no parent

        while (frontier.Count > 0)
        {
            var (currentPos, currentDir, turns) = frontier.Dequeue();

            if (currentPos == target)
            {
                List<(int x, int y)> pathToTarget = new();
                var step = target; //backtrack from target after reached it
                while (step != startPos) //go until reach start tile
                {
                    pathToTarget.Add(step);
                    step = cameFrom[step]; //backtrack to previous step
                }
                pathToTarget.Add(startPos); // Include the start tile
                pathToTarget.Reverse();      // Flip to get start-to-target order

                return pathToTarget;
            }

            foreach (var direction in Direction.NoDiagonal)
            {
                // Step one tile in the current direction
                (int nx, int ny) = (currentPos.x + direction.x, currentPos.y + direction.y);
                var nextPos = (nx, ny);

                if (IsBlocked(nextPos) && nextPos != target)
                {
                    continue;
                }
                    
                // Determine if next move changes direction (adds a turn)
                int nextTurns = currentDir.HasValue && currentDir.Value != direction
                    ? turns + 1 : turns;

                if (nextTurns > 2)
                {
                    continue;
                }

                if (visited.Contains((nextPos, direction))) //avoid revisiting same position in same direction
                {
                    continue;
                }

                frontier.Enqueue((nextPos, direction, nextTurns));
                visited.Add((nextPos, direction));

                // Save path tracking info if this is the first time reaching this tile
                if (!cameFrom.ContainsKey(nextPos))
                {
                    cameFrom[nextPos] = currentPos;
                }
            }
        }

        return null;
    }

    public static List<(int x, int)> Connect(MatchTile start, MatchTile target)
    {
        (int x, int y) startPost = (Mathf.FloorToInt(start.transform.position.x), Mathf.FloorToInt(start.transform.position.y));
        (int x, int y) targetPos = (Mathf.FloorToInt(target.transform.position.x), Mathf.FloorToInt(target.transform.position.y));

        Debug.Log($"TurnBFS: Start: {startPost}, Target: {targetPos}");
        return TurnBFS(startPost, targetPos);
    }


    private static bool IsBlocked((int x, int y) position)
    {
        if (position.x < -1 || position.x > rowLength || position.y < -1 || position.y > colLength) //out of bounds
        {
            return true;
        }

        if (position.x == -1 || position.x == rowLength || position.y == -1 || position.y == colLength) //search outside 1 for edge searches
        {
            return false;
        }

        var tile = board[position.x, position.y];
        return tile != null && tile.IsBlockable;
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