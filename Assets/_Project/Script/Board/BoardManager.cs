using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : Singleton<BoardManager>
{
    public Vector2Int boardSize = new Vector2Int(16, 6);
    public Tilemap boardTilemap;

    [SerializeField] private MatchTileListSO matchTileList;
    [SerializeField] private ObstacleTileListSO obstacleTileList;
    [SerializeField] private List<SpecialEffectData> specialEffects;

    [SerializeField] private GameTileFactorySO gameTileFactory;

    public GameTile[,] board;

    private int monsterTileIndex, itemTileIndex = 0;

    [Header("Debug")]
    [SerializeField] private Color borderColor = Color.red;

    void Awake()
    {
        Debug.Log("Initializing BoardManager...");
        board = new GameTile[boardSize.x, boardSize.y];
    }

    void Start()
    {
        FillBoard();
        
    }

    private void FillBoard()
    {
        int boardX = boardSize.x;
        int boardY = boardSize.y;

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

        int escape = 0;
        Vector3Int randomPos;
        Vector3 position;
        var monsterList = matchTileList.monsterTiles;
        int monsterCount = monsterList.Count; //cached
        int unfillCount = unfilledPositions.Count;
        while (unfillCount > 0)
        {
            if (escape++ > 1000)
            {
                Debug.LogError("Spawning tile pairs: infinite loop.");
                return;
            }

            if (unfillCount >= 2) //place by pairs
            {
                // place first tile
                randomPos = unfilledPositions.GetRandomElement();
                position = boardTilemap.GetCellCenterWorld(randomPos);
                gameTileFactory.CreateMonsterTile(monsterList[monsterTileIndex], position);
                unfilledPositions.Remove(randomPos);

                // place second tile
                randomPos = unfilledPositions.GetRandomElement();
                position = boardTilemap.GetCellCenterWorld(randomPos);
                gameTileFactory.CreateMonsterTile(monsterList[monsterTileIndex], position);
                unfilledPositions.Remove(randomPos);

                monsterTileIndex++;
                if (monsterTileIndex >= monsterCount)
                {
                    monsterTileIndex = 0;
                    monsterList.Shuffle();
                }
            }
        }
    }

    private void BorderPadding()
    {
        int boardX = boardSize.x + 1;
        int boardY = boardSize.y + 1;

        for (int x = -1; x < boardX; x++)
        {
            if (!IsOccupied(x, boardY - 1))
            {
                board[x, boardY - 1] = null;
            }

            if (!IsOccupied(x, -1))
            {
                board[x, -1] = null;
            }
        }
        for (int y = -1; y < boardY; y++)
        {
            if (!IsOccupied(-1, y))
            {
                board[-1, y] = null;
            }
            if (!IsOccupied(boardX - 1, y))
            {
                board[boardX - 1, y] = null;
            }
        }
    }

    public bool IsOccupied(int x, int y)
    {
        return board[x,y] != null;
    }

    public void PlaceGameTile(Vector3Int position, int tileIndex)
    {
        // boardTilemap.SetTile(position, gameTiles[tileIndex]);
    }

    #region Debugging
    private void OnDrawGizmos()
    {
        Vector3 center = new Vector3(boardSize.x / 2f, boardSize.y / 2f, 0);
        Vector3 size = new Vector3(boardSize.x, boardSize.y, 0f);
        Gizmos.color = borderColor;
        Gizmos.DrawWireCube(center, size);
        Gizmos.DrawWireCube(center + new Vector3(0.01f, 0), size);
        Gizmos.DrawWireCube(center - new Vector3(0.01f, 0), size);
        Gizmos.DrawWireCube(center + new Vector3(0, 0.01f), size);
        Gizmos.DrawWireCube(center - new Vector3(0, 0.01f), size);
    }


    #endregion

}
