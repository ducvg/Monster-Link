using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : Singleton<BoardManager>
{
    [field: SerializeField] public Vector2Int BoardSize { get; set; } = new Vector2Int(16, 6);
    [field: SerializeField] public Tilemap boardTilemap { get; private set; }
    [field: SerializeField] public GameObject highLightSelect  { get; set; }


    [SerializeField] private MatchTileListSO matchTiles;
    [SerializeField] private List<SpecialEffectData> specialEffects;

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
        TileConnection.OnInit();
        FillBoard();

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
        {
            switch (selectedTile1)
            {
                case null:
                    Debug.Log($"Selecting tile 1: {tile.name}");
                    selectedTile1 = tile;

                    break;

                case MatchTile t1 when t1 == tile:
                    Debug.Log($"Clicked the same tile again: {tile.name}, deselecting.");

                    selectedTile1.HighLightOff();
                    selectedTile1 = null;

                    break;

                case MatchTile t1 when t1.matchTileData == tile.matchTileData:
                    Debug.Log($"Connecting with tile 2: {tile.name}");
                    selectedTile2 = tile;

                    //Connect the tiles
                    var connectable = TileConnection.Connect(selectedTile1, selectedTile2);
                    Debug.Log(connectable != null  ? "Tiles connected successfully!" : "Failed to connect tiles.");

                    selectedTile1.HighLightOff();
                    selectedTile2.HighLightOff();
                    selectedTile1 = null;
                    selectedTile2 = null;
                    
                    break;

                default:
                    Debug.Log($"Resetting selection, clicked on a different tile: {tile.name}");
                    selectedTile1 = tile;
                    selectedTile1.HighLightOn();


                    break;
            }
        }
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
    }
    #endregion

}
