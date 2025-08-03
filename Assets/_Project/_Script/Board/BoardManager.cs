using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Utility.SkibidiTween;

public class BoardManager : Singleton<BoardManager>
{
    [field: SerializeField] public Vector2Int BoardSize { get; private set; } = new Vector2Int(15, 6);
    [field: SerializeField] public Tilemap BoardTilemap { get; private set; }
    [field: SerializeField] public float LevelTime { get; private set; } = 300f;

    public GravityDirection GravityDirection 
    {
        get => gravityDirection;
        set
        {
            gravityDirection = value;
        }
    }

    [SerializeField] private GravityDirection gravityDirection = GravityDirection.None;
    [SerializeField] private Skibidi<Vector2> boardMoveSetting;
    [SerializeField] private BoardGenerator boardGenerator;
    [SerializeField] private LineDrawerSO lineDrawer;

    public GameTile[,] board;
    public int MatchTileCount { get; set; }

    #region Debug
    [Header("Debug")]
    [SerializeField, InspectorName("Show board border")] private bool isDebugBorder = true;
    [SerializeField, InspectorName("Show board position")] private bool isDebugPosition = true;
    [SerializeField] private Color borderColor = Color.red;
#endregion

    void Awake()
    {
        board = new GameTile[BoardSize.x, BoardSize.y];
    }

    void Start()
    {
        boardGenerator.FillBoard();
        boardGenerator.GenerateTileEffects();
        GameBoard.ApplyGravityAll(GravityDirection, isSkipAnimation: true);

        MatchTileCount = GameBoard.GetMatchTiles().Count;

        if(GameBoard.FindAnyPath() == null) Shuffle();
        ShowBoard();
    }   

    public void AutoSolve()
    {
        
        var path = GameBoard.FindAnyPath();
        if (path != null)
        {
            lineDrawer.DrawLine(path);
        }
    }

    public void Shuffle()
    {
        TileSelector.Instance.Reset();
        GameBoard.Shuffle();
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
        var gridTransform = BoardTilemap.transform.parent;
        StartCoroutine(gridTransform.SkibidiMove
        (
            boardMoveSetting, 
            onComplete: () => gridTransform.gameObject.SetActive(false), 
            isReverse: false
        ));
    }

    private void ShowBoard()
    {
        var gridTransform = BoardTilemap.transform.parent;
        gridTransform.gameObject.SetActive(true);
        StartCoroutine(gridTransform.SkibidiMove(boardMoveSetting, null, isReverse: true));
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

public enum GravityDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}
