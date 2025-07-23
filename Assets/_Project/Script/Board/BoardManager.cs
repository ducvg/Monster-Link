using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : Singleton<BoardManager>
{
    [field: SerializeField] public Vector2Int BoardSize { get; private set; } = new Vector2Int(16, 6);
    [field: SerializeField] public Tilemap boardTilemap { get; private set; }
    [field: SerializeField] public GameObject highLightSelect  { get; set; }


    [SerializeField] private MatchTileListSO matchTiles;
    [SerializeField] private List<SpecialEffectData> specialEffects;

    [SerializeField] private LineFactorySO lineFactory;
    [SerializeField] private GameTileFactorySO gameTileFactory;

    public GameTile[,] board;
    public MatchTile selectedTile1, selectedTile2;

    [Header("Debug")]
    [SerializeField] private Color borderColor = Color.red;

    void Awake()
    {
        Debug.Log("Initializing BoardManager...");
        board = new GameTile[BoardSize.x, BoardSize.y];
    }

    void Start()
    {
        GameBoard.OnInit();
        FillBoard();
        Shuffle();
    }   

    [ContextMenu("Shuffle board")]
    void Shuffle()
    {
        Debug.Log("Shuffling tiles...");
        GameBoard.Shuffle();   
    }

    private void FillBoard()
    {
        int boardX = BoardSize.x;
        int boardY = BoardSize.y;

        List<Vector3Int> unfilledPositions = new();
        Vector3Int tilePosition;
        for (int x = 0; x < boardX; x++)
        {
            for (int y = 0; y < boardY; y++)
            {
                tilePosition = new Vector3Int(x, y);
                if (!IsOccupied(x, y))
                {
                    unfilledPositions.Add(tilePosition);
                }
            }
        }

        Vector3Int randomPos;
        Vector3 position;
        while (unfilledPositions.Count > 0)
        {
            if (unfilledPositions.Count >= 2) //place monster by pairs
            {
                // place first tile
                randomPos = unfilledPositions.GetRandomElement();
                position = boardTilemap.GetCellCenterWorld(randomPos);
                gameTileFactory.CreateMonsterTile(matchTiles.GetCurrentMonsterTile(), position);
                unfilledPositions.Remove(randomPos);

                // place second tile
                randomPos = unfilledPositions.GetRandomElement();
                position = boardTilemap.GetCellCenterWorld(randomPos);
                gameTileFactory.CreateMonsterTile(matchTiles.GetCurrentMonsterTile(), position);
                unfilledPositions.Remove(randomPos);

                matchTiles.NextMonsterIndex();
            }
            else
            {
                return;
            }
        }
    }

    public void SelectTile(MatchTile tile)
    {
        tile.HighLightOn();
        if (selectedTile1 == null) // select 1st tile
        {
            Debug.Log($"Selecting tile 1");
            selectedTile1 = tile;
        }
        else if (selectedTile1 == tile) //  select same tile
        {
            Debug.Log($"Clicked the same tile, deselect.");
            selectedTile1.HighLightOff();
            selectedTile1 = null;
        }
        else if (selectedTile1.matchTileData == tile.matchTileData) // select 2nd tile of same 1st tile type
        {
            Debug.Log($"Connecting with tile 2");
            selectedTile2 = tile;

            // Connect the tiles
            List<(int x, int y)> path = GameBoard.Connect(selectedTile1, selectedTile2);
            if (path != null)
            {
                Debug.Log($"Connectable !");
                Line line = DrawLine(path);
                line.OnDespawn();

                selectedTile1.OnConnect();
                selectedTile2.OnConnect();

            }
            else
            {
                selectedTile1.HighLightOff();
                selectedTile2.HighLightOff();
            }


            selectedTile1 = null;
            selectedTile2 = null;
        }
        else // select 2nd tile of different type (tile1 != null && tile1 != tile && tile1.matchTileData != tile.matchTileData)
        {
            Debug.Log($"Reset selection, clicked on a different tile");

            selectedTile1.HighLightOff();
            selectedTile1 = tile;
            selectedTile1.HighLightOn();
        }
        
    }

    public void EnsureSolvable()
    {
        while (GameBoard.FindAnyPath() == null)
        {
            
        }
    }

    public Line DrawLine(List<(int x, int y)> path)
    {
        Vector3[] pathVector = new Vector3[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            Vector3Int tilePosition = new Vector3Int(path[i].x, path[i].y, 0);
            pathVector[i] = boardTilemap.GetCellCenterWorld(tilePosition);
        }

        Line line = lineFactory.CreateLine(pathVector);

        return line;
    }

    public void RemoveTile(GameTile tile)
    {
        if (tile == null) return;

        Vector3Int tilePosition = boardTilemap.WorldToCell(tile.transform.position);
        int x = tilePosition.x;
        int y = tilePosition.y;

        if (x < 0 || x >= BoardSize.x || y < 0 || y >= BoardSize.y)
        {
            Debug.LogError($"RemoveTile: Coordinates ({x}, {y}) are out of bounds.");
            return;
        }

        board[x, y] = null;
        Destroy(tile.gameObject);
    }

    public GameTile GetTile(int x, int y)
    {
        if (x < 0 || x >= BoardSize.x || y < 0 || y >= BoardSize.y)
        {
            Debug.LogError($"GetTile: Coordinates ({x}, {y}) are out of bounds.");
            return null;
        }
        return board[x, y];
    }

    public bool IsOccupied(int x, int y)
    {
        return board[x, y] != null;
    }

    #region Debugging
    private void OnDrawGizmos()
    {
        Vector3 center = new Vector3(BoardSize.x / 2f, BoardSize.y / 2f, 0);
        Vector3 size = new Vector3(BoardSize.x, BoardSize.y, 0f);
        Gizmos.color = borderColor;
        Gizmos.DrawWireCube(center, size);
        Gizmos.DrawWireCube(center + new Vector3(0.01f, 0), size);
        Gizmos.DrawWireCube(center - new Vector3(0.01f, 0), size);
        Gizmos.DrawWireCube(center + new Vector3(0, 0.01f), size);
        Gizmos.DrawWireCube(center - new Vector3(0, 0.01f), size);

        // int rowLength = BoardSize.x;
        // int colLength = BoardSize.y;
        
        // if(Application.isPlaying)
        // {
        //     for (int i = 0; i < rowLength; i++)
        //     {
        //         for (int j = 0; j < colLength; j++)
        //         {
        //             if (board[i, j] != null)
        //             {
        //                 Gizmos.color = Color.red;
        //                 Gizmos.DrawCube(new Vector3(i + 0.5f, j + 0.5f, 0), new Vector3(0.2f, 0.2f, 0));
        //             }
        //         }
        //     }
        // }
        
    }
    #endregion

}
