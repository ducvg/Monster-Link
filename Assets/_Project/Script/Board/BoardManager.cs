using System;
using System.Collections.Generic;
using NUnit.Framework;
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

    public Dictionary<Vector3, GameTile> board = new();

    private int monsterTileIndex, itemTileIndex = 0;

    [Header("Debug")]
    [SerializeField] private Color borderColor = Color.red;

    void Awake()
    {
        if (boardSize.x % 2 != 0 && boardSize.y % 2 != 0)
        {
            boardSize.x -= 1; // Ensure pairs to match
        }
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
        for (int x = 0; x < boardX; x++)
        {
            for (int y = 0; y < boardY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y);
                if (!IsOccupied(tilePosition))
                {
                    unfilledPositions.Add(tilePosition);
                }
            }
        }

        int escape = 0;
        Vector3Int randomPos;
        var monsterList = matchTileList.monsterTiles;
        while (unfilledPositions.Count > 0)
        {
            if (escape++ > 1000)
            {
                Debug.LogError("Spawning tile pairs: infinite loop.");
                return;
            }

            if (unfilledPositions.Count >= 2) //place by pairs
            {
                // place first tile
                randomPos = unfilledPositions.GetRandomElement();
                var t = gameTileFactory.CreateMonsterTile(monsterList[monsterTileIndex]);
                t.transform.position = boardTilemap.GetCellCenterWorld(randomPos);
                unfilledPositions.Remove(randomPos);

                // place second tile
                randomPos = unfilledPositions.GetRandomElement();
                t = gameTileFactory.CreateMonsterTile(monsterList[monsterTileIndex]);
                t.transform.position = boardTilemap.GetCellCenterWorld(randomPos);
                unfilledPositions.Remove(randomPos);

                monsterTileIndex++;
                if (monsterTileIndex >= monsterList.Count)
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
            Vector3Int top = new Vector3Int(x, boardY - 1);
            if (!IsOccupied(top))
            {
                board[top] = null;
            }
            Vector3Int bottom = new Vector3Int(x, -1);
            if (!IsOccupied(bottom))
            {
                board[bottom] = null;
            }
        }
        for (int y = -1; y < boardY; y++)
        {
            Vector3Int right = new Vector3Int(-1, y);
            if (!IsOccupied(right))
            {
                board[right] = null;
            }
            Vector3Int left = new Vector3Int(boardX - 1, y);
            if (!IsOccupied(left))
            {
                board[left] = null;
            }
        }
    }

    public bool IsOccupied(Vector3Int position)
    {
        return board.ContainsKey(position) && board[position] != null;
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
