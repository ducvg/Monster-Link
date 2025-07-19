using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class MatchTile : GameTile, IPointerClickHandler
{
    protected IObjectPool<MatchTile> pool;
    public IObjectPool<MatchTile> Pool
    {
        get => pool;
        set => pool = value;
    }

    public SpriteRenderer Icon
    {
        get => icon;
        private set => icon = value;
    }
    public SpecialEffectData TileEffect //exposed for random gen
    {
        get => tileEffect;
        set => tileEffect = value;
    }
    public Animator Animator
    {
        get => animator;
        private set => animator = value;
    }

    [Header("Match Tile Properties")]
    [SerializeField] private SpecialEffectData tileEffect;

    [SerializeField] private SpriteRenderer background, icon;
    [SerializeField] private Animator animator;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnInit()
    {
        base.OnInit();

        //remove effect
        tileEffect = null;
        background.color = Color.white;


    }

    public void CopyFrom(MatchTile prefab)
    {
        icon.sprite = prefab.Icon.sprite;
        animator.runtimeAnimatorController = prefab.Animator.runtimeAnimatorController;

        if(prefab.TileEffect != null)
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
        tileEffect = effectData;
        background.color = tileEffect.tileColor;
    }

    public void ApplyEffects()
    {
        tileEffect.ApplyEffect();
    }
    
    public override void OnDespawn()
    {
        base.OnDespawn();
        
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
