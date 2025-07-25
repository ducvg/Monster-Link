using UnityEngine;

public class GameTile : MonoBehaviour
{
    [field: Header("Game tile Behavior")]
    [field: SerializeField] public bool IsBlockable { get; private set; } = true;
    [field: SerializeField] public bool IsMovable { get; private set; } = true;

    public Vector3Int BoardPosition { get; set; }


    protected virtual void Awake()
    {
        OnInit();
    }

    protected virtual void Start()
    {

    }

    public virtual void OnInit()
    {
        var position = transform.position;
        //convert to tilemap position
        BoardPosition = BoardManager.Instance.BoardTilemap.WorldToCell(position);
        // if designer hand placed the tile in the scene, register it to the board to save it
        BoardManager.Instance.board[BoardPosition.x, BoardPosition.y] = this;
    }

    public virtual void OnDespawn()
    {
        
    }
}