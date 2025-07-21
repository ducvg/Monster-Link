using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class MatchTile : GameTile, IPointerClickHandler
{
    public IObjectPool<MatchTile> Pool { get; set; }

    [Header("Match Tile Properties")]
    [field: SerializeField] public MatchTileData matchTileData { get; set; }
    [field: SerializeField] public SpecialEffectData TileEffect { get; set; }
    [field: SerializeField] public SpriteRenderer Background { get; private set; }
    [field: SerializeField] public SpriteRenderer Icon { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    private int currentAnim;

    protected override void Awake()
    {
        base.Awake();

    }

    public override void OnInit()
    {
        base.OnInit();

        //remove effect
        TileEffect = null;
        Background.color = Color.white;

    }

    public void CopyFrom(MatchTile prefab)
    {
        Icon.sprite = prefab.Icon.sprite;
        Animator.runtimeAnimatorController = prefab.Animator.runtimeAnimatorController;

        if (prefab.TileEffect != null)
        {
            SetEffect(prefab.TileEffect);
        }
    }

    protected override void Start()
    {
        base.Start();

    }

    public void SetEffect(SpecialEffectData effectData)
    {
        TileEffect = effectData;
        Background.color = TileEffect.tileColor;
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
        BoardManager.Instance.SelectTile(this);
        HighLightOn();
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

    private void ChangeAnim(int animHash)
    {
        if(currentAnim != 0) Animator.ResetTrigger(currentAnim);
        currentAnim = animHash;
        Animator.SetTrigger(currentAnim);
    }
}
