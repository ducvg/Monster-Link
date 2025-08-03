using System.Collections.Generic;
using UnityEngine;
using Utility.SkibidiTween;
using Random = UnityEngine.Random;

public static class GameBoard
{

#region Shuffle
    public static void Shuffle() //swapping positions of match tiles
    {
        List<MatchTile> matchTiles = GetMatchTiles();

        if(matchTiles.Count == 0) return;

        int escape = 0;
        int tileCount = matchTiles.Count;
        List<MatchTile> matchTilesCopy;
        do
        {
            matchTilesCopy = new(matchTiles);
            int rolls = (int)Random.Range(tileCount * 0.5f, tileCount);
            while (rolls-- > 0)
            {
                if (matchTilesCopy.Count < 2) break;
                
                var tile1 = matchTilesCopy.GetRandomElement();
                matchTilesCopy.Remove(tile1);
                var tile2 = matchTilesCopy.GetRandomElement();
                matchTilesCopy.Remove(tile2);

                tile1.lineDespawnAction?.Invoke();
                tile2.lineDespawnAction?.Invoke();

                SwapTiles(tile1, tile2);
            }
        } while (FindAnyPath() == null && escape++ < 100);

        if(escape >= 100)
        {
            ShuffleRandomPosition();
        }
    }

    public static void ShuffleRandomPosition()
    {
        List<Vector3Int> validPositions = GetPositionsExceptType<ObstacleTile>();
        if(validPositions.Count < 2) return;

        int escape = 0;
        int posCount = validPositions.Count;
        List<Vector3Int> validPositionsCopy;
        do
        {
            validPositionsCopy = new(validPositions);
            int rolls = (int)Random.Range(posCount * 0.5f, posCount);
            while (rolls-- > 0)
            {
                if (validPositionsCopy.Count < 2) break;
                
                var pos1 = validPositionsCopy.GetRandomElement();
                validPositionsCopy.Remove(pos1);
                var pos2 = validPositionsCopy.GetRandomElement();
                validPositionsCopy.Remove(pos2);

                var tile1 = BoardManager.Instance.board[pos1.x, pos1.y];
                var tile2 = BoardManager.Instance.board[pos2.x, pos2.y];

                SwapBoardPositions(pos1, pos2);

                if(tile1 is MatchTile matchTile1) matchTile1.lineDespawnAction?.Invoke();
                if(tile2 is MatchTile matchTile2) matchTile2.lineDespawnAction?.Invoke();
            }
            ApplyGravityAll(BoardManager.Instance.GravityDirection, isSkipAnimation: true);
        } while (FindAnyPath() == null && escape++ < 100);
    }

    private static void SwapTiles(GameTile tile1, GameTile tile2)
    {
        var board = BoardManager.Instance.board;

        (board[tile1.BoardPosition.x, tile1.BoardPosition.y], board[tile2.BoardPosition.x, tile2.BoardPosition.y]) = (tile2, tile1);
        (tile2.transform.position, tile1.transform.position) = (tile1.transform.position, tile2.transform.position);
        (tile2.BoardPosition, tile1.BoardPosition) = (tile1.BoardPosition, tile2.BoardPosition);
    }

    private static void SwapBoardPositions(Vector3Int pos1, Vector3Int pos2)
    {
        var board = BoardManager.Instance.board;
        var tilemap = BoardManager.Instance.BoardTilemap;

        var tile1 = board[pos1.x, pos1.y];
        var tile2 = board[pos2.x, pos2.y];

        // Swap in board array
        board[pos1.x, pos1.y] = tile2;
        board[pos2.x, pos2.y] = tile1;

        // Update board positions
        if (tile1 != null)
        {
            tile1.BoardPosition = pos2;
            tile1.transform.position = tilemap.GetCellCenterWorld(pos2);
        }

        if (tile2 != null)
        {
            tile2.BoardPosition = pos1;
            tile2.transform.position = tilemap.GetCellCenterWorld(pos1);
        }
    }
#endregion

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
 
#region Apply Gravity
    public static void ApplyGravityAll(GravityDirection gravityDirection, bool isSkipAnimation)
    {
        if(gravityDirection == GravityDirection.None) return;

        var xLength = BoardManager.Instance.BoardSize.x;
        var yLength = BoardManager.Instance.BoardSize.y;

        switch (gravityDirection)
        {
            case GravityDirection.Up:
                for(int x = 0; x < xLength; x++) 
                    for(int y = 0; y < yLength; y++)
                        ApplyGravityUpAt(x, isSkipAnimation); //apply gravity up each column y times
                break;
            case GravityDirection.Down:
                for(int x = 0; x < xLength; x++) 
                    for(int y = 0; y < yLength; y++)
                        ApplyGravityDownAt(x, isSkipAnimation); 
                break;
            case GravityDirection.Left:
                for(int y = 0; y < yLength; y++) 
                    for(int x = 0; x < xLength; x++)
                        ApplyGravityLeftAt(y, isSkipAnimation); //apply gravity left each row x times
                break;
            case GravityDirection.Right:
                for(int y = 0; y < yLength; y++) 
                    for(int x = 0; x < xLength; x++)
                        ApplyGravityRightAt(y, isSkipAnimation);
                break;
        }
    }

    public static void ApplyGravityAt(GameTile tileConnected, GravityDirection gravityDirection)
    {
        if(gravityDirection == GravityDirection.None) return;
        
        var colLength = BoardManager.Instance.BoardSize.y;
        var rowLength = BoardManager.Instance.BoardSize.x;

        switch (gravityDirection)
        {
            case GravityDirection.Up:
                for(int y = 0; y < colLength; y++)
                    ApplyGravityUpAt(tileConnected.BoardPosition.x); //apply gravity up in tile's column y times
                break;
            case GravityDirection.Down:
                for(int y = 0; y < colLength; y++)
                    ApplyGravityDownAt(tileConnected.BoardPosition.x);
                break;
            case GravityDirection.Left:
                for(int x = 0; x < rowLength; x++)
                    ApplyGravityLeftAt(tileConnected.BoardPosition.y); //apply gravity left in tile's row x times
                break;
            case GravityDirection.Right:
                for(int x = 0; x < rowLength; x++)
                    ApplyGravityRightAt(tileConnected.BoardPosition.y);
                break;
        }
    }

    private static void ApplyGravityUpAt(int colIndex, bool isSkipAnimation = false) //pull all tiles up this in this tile column
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var tilemap = BoardManager.Instance.BoardTilemap;
        int x = colIndex;

        var cacheVec = Vector3Int.zero;
        for (int y = colLength - 1; y > 0; y--)
        {
            if(board[x, y] != null) continue;
            GameTile tileToPull = board[x, y-1];
            if(tileToPull == null || !tileToPull.IsMovable) continue;

            cacheVec.x = x; 
            cacheVec.y = y;

            if(!isSkipAnimation) tileToPull.MoveTo(tilemap.GetCellCenterWorld(cacheVec));
            else tileToPull.transform.position = tilemap.GetCellCenterWorld(cacheVec);
            tileToPull.BoardPosition = cacheVec;
            board[x, y] = tileToPull;
            board[x, y-1] = null;
        }
        
    }

    private static void ApplyGravityDownAt(int colIndex, bool isSkipAnimation = false)
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var tilemap = BoardManager.Instance.BoardTilemap;
        int x = colIndex;

        var cacheVec = Vector3Int.zero;
        for (int y = 0; y < colLength - 1; y++)
        {
            if(board[x, y] != null) continue;
            GameTile tileToPush = board[x, y+1];
            if(tileToPush == null || !tileToPush.IsMovable) continue;

            cacheVec.x = x;
            cacheVec.y = y;

            if(!isSkipAnimation) tileToPush.MoveTo(tilemap.GetCellCenterWorld(cacheVec));
            else tileToPush.transform.position = tilemap.GetCellCenterWorld(cacheVec);
            tileToPush.BoardPosition = cacheVec;
            board[x, y] = tileToPush;
            board[x, y+1] = null;
        }
    }

    private static void ApplyGravityLeftAt(int rowIndex, bool isSkipAnimation = false)
    {
        var board = BoardManager.Instance.board;
        var rowLength = BoardManager.Instance.BoardSize.x;
        var tilemap = BoardManager.Instance.BoardTilemap;
        int y = rowIndex;

        var cacheVec = Vector3Int.zero;
        for (int x = 0; x < rowLength - 1; x++)
        {
            GameTile leftTile = board[x, y];
            if(leftTile != null) continue;
            GameTile tileToPush = board[x+1, y];
            if(tileToPush == null || !tileToPush.IsMovable) continue;

            cacheVec.x = x;
            cacheVec.y = y;

            if(!isSkipAnimation) tileToPush.MoveTo(tilemap.GetCellCenterWorld(cacheVec));
            else tileToPush.transform.position = tilemap.GetCellCenterWorld(cacheVec);
            tileToPush.BoardPosition = cacheVec;
            board[x, y] = tileToPush;
            board[x+1, y] = null;
        }
    }   

    private static void ApplyGravityRightAt(int rowIndex, bool isSkipAnimation = false)
    {
        var board = BoardManager.Instance.board;
        var rowLength = BoardManager.Instance.BoardSize.x;
        var tilemap = BoardManager.Instance.BoardTilemap;
        int y = rowIndex;

        var cacheVec = Vector3Int.zero;
        for (int x = rowLength - 1; x > 0; x--)
        {
            GameTile rightTile = board[x, y];
            if(rightTile != null) continue;
            GameTile tileToPush = board[x-1, y];
            if(tileToPush == null || !tileToPush.IsMovable) continue;

            cacheVec.x = x;
            cacheVec.y = y;

            if(!isSkipAnimation) tileToPush.MoveTo(tilemap.GetCellCenterWorld(cacheVec));
            else tileToPush.transform.position = tilemap.GetCellCenterWorld(cacheVec);
            tileToPush.BoardPosition = cacheVec;
            board[x, y] = tileToPush;
            board[x-1, y] = null;
        }
    }
#endregion

#region Helpers
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

    public static List<Vector3Int> GetPositionsExceptType<T>() where T : GameTile
    {
        var board = BoardManager.Instance.board;
        var colLength = BoardManager.Instance.BoardSize.y;
        var rowLength = BoardManager.Instance.BoardSize.x;

        List<Vector3Int> positions = new(); 
        
        for(int x = 0; x < rowLength; x++)
        {
            for(int y = 0; y < colLength; y++)
            {
                if(board[x, y] is not T) positions.Add(new Vector3Int(x, y, 0));
            }
        }

        return positions;
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

}
