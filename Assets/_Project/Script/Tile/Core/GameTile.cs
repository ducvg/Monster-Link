using UnityEngine;

public class GameTile : MonoBehaviour
{
    protected virtual void Awake()
    {
        var position = transform.position;
        //convert to tilemap position
        var tilePosition = BoardManager.Instance.boardTilemap.WorldToCell(position);
        // if designer hand placed the tile in the scene, register it to the board to save it
        BoardManager.Instance.board[tilePosition] = this;
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
