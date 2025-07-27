using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;
using Utility.SkibidiTween;

public class BoardManager : Singleton<BoardManager>
{
    [field: SerializeField] public Vector2Int BoardSize { get; private set; } = new Vector2Int(15, 6);
    [field: SerializeField] public Tilemap BoardTilemap { get; private set; }
    [field: SerializeField] public GameObject HighLightSelect  { get; set; }

    [SerializeField] private Skibidi<Vector2> boardMoveSetting;

    [Header("Factory")]
    [SerializeField] private LineDrawerSO lineDrawer;
    [SerializeField] private GameTileFactorySO gameTileFactory;

    [Header("Storage")]
    [SerializeField] private MatchTileListSO matchTiles;
    [SerializeField] private List<TileEffectData> specialEffects;

    public GameTile[,] board;
    private MatchTile selectedTile1, selectedTile2;

#region Debug
    [Header("Debug")]
    [SerializeField, InspectorName("Show board border")] private bool isDebugBorder = true;
    [SerializeField, InspectorName("Show board position")] private bool isDebugPosition = true;
    [SerializeField] private Color borderColor = Color.red;
#endregion

    void Awake()
    {
        Debug.Log("Initializing BoardManager...");
        board = new GameTile[BoardSize.x, BoardSize.y];
    }

    void Start()
    {
        ShowBoard();
        GameBoard.OnInit();
        FillBoard();
        if(GameBoard.FindAnyPath() == null) Shuffle();
    }   

    public void AutoSolve()
    {
        var path = GameBoard.FindAnyPath();
        if (path != null)
        {
            lineDrawer.DrawLine(path, 3f);
        }
    }

    public void Shuffle()
    {
        GameBoard.Shuffle();
    }

    private void FillBoard()
    {
        List<(int x, int y)> unfilledPositions = GameBoard.GetEmptyPositions();

        (int x, int y) randomPos;
        Vector3 position;
        while (unfilledPositions.Count > 0)
        {
            if (unfilledPositions.Count >= 2) //place monster by pairs
            {
                // place first tile
                randomPos = unfilledPositions.GetRandomElement();
                position = BoardTilemap.GetCellCenterWorld(new Vector3Int(randomPos.x, randomPos.y));
                gameTileFactory.CreateMonsterTile(matchTiles.GetCurrentMonsterTile(), position, BoardTilemap.transform);
                unfilledPositions.Remove(randomPos);

                // place second tile
                randomPos = unfilledPositions.GetRandomElement();
                position = BoardTilemap.GetCellCenterWorld(new Vector3Int(randomPos.x, randomPos.y));
                gameTileFactory.CreateMonsterTile(matchTiles.GetCurrentMonsterTile(), position, BoardTilemap.transform);
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
                lineDrawer.DrawLine(path, 1f);

                OnTilesConnected();
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

    public void OnTilesConnected()
    {
        selectedTile1.OnConnect();
        selectedTile2.OnConnect();
    }


    public void ClearBoard()
    {
        int rowLength = BoardSize.x;
        int colLength = BoardSize.y;
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                var tile = board[i, j];
                if(tile != null)
                {
                    tile.OnDespawn();
                    board[i, j] = null;
                }
            }
        }
    }

    private void HideBoard()
    {
        StartCoroutine(BoardTilemap.transform.parent.SkibidiMove
        (
            boardMoveSetting, 
            onComplete: () => BoardTilemap.transform.parent.gameObject.SetActive(false), 
            isReverse: false
        ));
    }

    private void ShowBoard()
    {
        BoardTilemap.transform.parent.gameObject.SetActive(true);
        StartCoroutine(BoardTilemap.transform.parent.SkibidiMove(boardMoveSetting, null, isReverse: true));
    }

    void OnEnable()
    {
        GameState.OnGamePause += HideBoard;
        GameState.OnGameResume += ShowBoard;
    }



    void OnDisable()
    {
        GameState.OnGamePause -= HideBoard;
        GameState.OnGameResume -= ShowBoard;
    }

    #region Debugging
    private void OnDrawGizmos()
    {
        if(isDebugBorder)
        {
            Vector3 center = new (BoardSize.x / 2f, BoardSize.y / 2f, 0);
            Vector3 size = new (BoardSize.x, BoardSize.y, 0f);
            Gizmos.color = borderColor;
            Gizmos.DrawWireCube(center, size);
            Gizmos.DrawWireCube(center + new Vector3(0.01f, 0), size);
            Gizmos.DrawWireCube(center - new Vector3(0.01f, 0), size);
            Gizmos.DrawWireCube(center + new Vector3(0, 0.01f), size);
            Gizmos.DrawWireCube(center - new Vector3(0, 0.01f), size);
        }

        if(isDebugPosition)
        {
            int rowLength = BoardSize.x;
            int colLength = BoardSize.y;
            
            if(Application.isPlaying)
            {
                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {
                        if (board[i, j] != null)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(new Vector3(i + 0.5f, j + 0.5f, 0), new Vector3(0.2f, 0.2f, 0));
                        }
                    }
                }
            }    
        }

        
        
    }

    [ContextMenu("Set board as show")]
    private void SetBoardAsShow()
    {
        BoardTilemap.transform.parent.position = boardMoveSetting.start;
    }

    [ContextMenu("Set board as hide")]
    private void SetBoardAsHide()
    {
        BoardTilemap.transform.parent.position = boardMoveSetting.end;
    }
    #endregion

}
