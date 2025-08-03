using System;
using UnityEngine;
using Utility.SkibidiTween;

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
        BoardPosition = BoardManager.Instance.BoardTilemap.WorldToCell(position);
        BoardManager.Instance.board[BoardPosition.x, BoardPosition.y] = this;
    }

    public virtual void MoveTo(Vector3 destination, float speed = 4f, Action onComplete = null, Action onUpdate = null)
    {
        onComplete += OnMoveComplete;
        StartCoroutine(transform.SkibidiMoveAtSpeed(destination, speed, onComplete, onUpdate));
    }

    protected virtual void OnMoveComplete()
    {
    }

    public virtual void OnDespawn()
    {
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }
}