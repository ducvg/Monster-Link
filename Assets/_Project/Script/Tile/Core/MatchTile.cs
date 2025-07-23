using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class MatchTile : GameTile, IPointerClickHandler
{
    public IObjectPool<MatchTile> Pool { get; set; }

    [field: Header("Match Tile Properties")]
    [field: SerializeField] public MatchTileData matchTileData { get; set; }
    [SerializeField] private SpecialEffectData tileEffect;
    public SpecialEffectData TileEffect
    {
        get => tileEffect;
        set
        {
            tileEffect = value;
            if (tileEffect != null)
            {
                Background.color = value.tileColor;
            } else
            {
                Background.color = Color.white;
            }
        }
    }
    [field: SerializeField] public SpriteRenderer Background { get; private set; }
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

        //remove effect
        isInteractable = true;
        TileEffect = null;
        Background.color = Color.white;

    }

    public void CopyFrom(MatchTile prefab)
    {
        BoardPosition = prefab.BoardPosition;
        Icon.sprite = prefab.Icon.sprite;
        Animator.runtimeAnimatorController = prefab.Animator.runtimeAnimatorController;
        TileEffect = prefab.TileEffect;
    }

    public void ApplyEffects()
    {
        TileEffect.ApplyEffect();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();

        Pool?.Release(this);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable) return;

        BoardManager.Instance.SelectTile(this);
    }

    public virtual void HighLightOn()
    {
        BoardManager.Instance.highLightSelect.SetActive(true);
        BoardManager.Instance.highLightSelect.transform.position = transform.position;

        ChangeAnim(AnimationHash.OnSelect);

    }

    public virtual void HighLightOff()
    {
        BoardManager.Instance.highLightSelect.SetActive(false);

        ChangeAnim(AnimationHash.OnDeselect);
    }

    public virtual void OnConnect()
    {
        isInteractable = false;
        BoardManager.Instance.highLightSelect.SetActive(false);
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
