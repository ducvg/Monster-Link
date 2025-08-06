using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class MatchTile : GameTile, IPointerClickHandler
{
    public IObjectPool<MatchTile> Pool { get; set; }

    [field: Header("Match Tile Properties")]
    [field: SerializeField] public MatchTileData MatchTileData { get; set; }
    public TileEffect TileEffect { get => tileEffect; set => tileEffect = value; }
    [field: SerializeField] public SpriteRenderer Icon { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    [SerializeField] private TileEffect tileEffect;

    [SerializeField] protected bool isInteractable = true;
    protected bool isConnected = false;

    public Action lineDespawnAction;
    private int currentAnim;
    private new Collider2D collider;

    protected override void Awake()
    {
        
        base.Awake();
        collider = GetComponent<Collider2D>();
    }

    public override void OnInit()
    {
        base.OnInit();

        isInteractable = true;
        tileEffect = null;
        GameState.OnGamePause += DisableInteraction;
        GameState.OnGameResume += EnableInteraction;

    }

    protected void EnableInteraction() => isInteractable = true;
    protected void DisableInteraction() => isInteractable = false;

    public void ApplyEffect()
    {
        if (tileEffect != null)
        {
            tileEffect.ApplyEffect();
            tileEffect.OnDespawn(this);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable) return;

        TileSelector.Instance.SelectTile(this);
    }

    public virtual void OnSelect()
    {
        
        ChangeAnim(AnimationHash.OnSelect);
    }

    public virtual void OnDeselect()
    {
        
        ChangeAnim(AnimationHash.OnDeselect);
    }

    public virtual void OnConnect()
    {
        isInteractable = false;
        isConnected = true;
        transform.position = transform.position.WithZ(-1f); //show above otherr
        ApplyEffect();

        BoardManager.Instance.board[BoardPosition.x, BoardPosition.y] = null;

        lineDespawnAction?.Invoke();
        ChangeAnim(AnimationHash.OnConnect);
        OnDespawn();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();

        collider.enabled = false;
        GameState.OnGamePause -= DisableInteraction;
        GameState.OnGameResume -= EnableInteraction;

        if(tileEffect != null) tileEffect.OnDespawn(this);
    
        Destroy(gameObject, 1.2f);
    }

    public override void MoveTo(Vector3 destination, float speed = 5f, Action onComplete = null, Action onUpdate = null)
    {
        if(isConnected) return;

        GamePowerState.isAllow = false;  //disable powers when board is moving
        isInteractable = false;
        base.MoveTo(destination, speed, onComplete, () => GamePowerState.isAllow = false);
    }

    protected override void OnMoveComplete()
    {
        base.OnMoveComplete();
        GamePowerState.isAllow = true;
        isInteractable = true;
    }

    protected void ChangeAnim(int animHash)
    {
        if (currentAnim != 0) Animator.ResetTrigger(currentAnim);
        currentAnim = animHash;
        Animator.SetTrigger(currentAnim);
    }
    
}
