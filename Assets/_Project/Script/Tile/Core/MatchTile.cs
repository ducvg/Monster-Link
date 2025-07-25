using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class MatchTile : GameTile, IPointerClickHandler
{
    public IObjectPool<MatchTile> Pool { get; set; }

    [field: Header("Match Tile Properties")]
    [field: SerializeField] public MatchTileData matchTileData { get; set; }
    [SerializeField] private TileEffectData tileEffect;
    public TileEffectData TileEffect
    {
        get => tileEffect;
        set
        {
            if(value == null) return;

            tileEffect = value;
            tileEffect.OnInit(this);
        }
    }
    [field: SerializeField] public SpriteRenderer Icon { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    protected bool isInteractable = true;

    private int currentAnim;

    protected override void Awake()
    {
        base.Awake();

    }

    public override void OnInit()
    {
        base.OnInit();

        isInteractable = true;
        tileEffect = null;

    }

    public void CopyFrom(MatchTile prefab)
    {
        // BoardPosition = prefab.BoardPosition;
        Icon.sprite = prefab.Icon.sprite;
        Animator.runtimeAnimatorController = prefab.Animator.runtimeAnimatorController;
        tileEffect = prefab.TileEffect;
    }

    public void ApplyEffects()
    {
        if (tileEffect != null)
        {
            tileEffect.ApplyEffect();

        }
    }

    public override void OnDespawn()
    {
        base.OnDespawn();

        if(tileEffect != null) tileEffect.OnDespawn(this);

        Pool?.Release(this);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable) return;

        BoardManager.Instance.SelectTile(this);
    }

    public virtual void HighLightOn()
    {
        BoardManager.Instance.HighLightSelect.SetActive(true);
        BoardManager.Instance.HighLightSelect.transform.position = transform.position;

        ChangeAnim(AnimationHash.OnSelect);

    }

    public virtual void HighLightOff()
    {
        BoardManager.Instance.HighLightSelect.SetActive(false);

        ChangeAnim(AnimationHash.OnDeselect);
    }

    public virtual void OnConnect()
    {
        isInteractable = false;
        BoardManager.Instance.HighLightSelect.SetActive(false);
        BoardManager.Instance.board[BoardPosition.x, BoardPosition.y] = null;

        ChangeAnim(AnimationHash.OnConnect);
    }

    protected void ChangeAnim(int animHash)
    {
        if (currentAnim != 0) Animator.ResetTrigger(currentAnim);
        currentAnim = animHash;
        Animator.SetTrigger(currentAnim);
    }
}
