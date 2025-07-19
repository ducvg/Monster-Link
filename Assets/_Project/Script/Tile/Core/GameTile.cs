using UnityEngine;

public class GameTile : MonoBehaviour
{
    [field: Header("Game tile Behavior")]
    [field: SerializeField] public bool IsBlockable { get; private set; } = true;
    [field: SerializeField] public bool IsMovable { get; private set; } = true;

    protected virtual void Awake()
    {
        var position = transform.position;
        //convert to tilemap position
        var tilePosition = BoardManager.Instance.boardTilemap.WorldToCell(position);
        // if designer hand placed the tile in the scene, register it to the board to save it
        BoardManager.Instance.board[tilePosition.x, tilePosition.y] = this;
    }

    protected virtual void Start()
    {

    }

    public virtual void OnInit()
    {

    }

    public virtual void OnDespawn()
    {
        
    }
}
