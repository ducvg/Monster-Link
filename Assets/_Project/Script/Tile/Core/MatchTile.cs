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
        set => icon = value;
    }
    public SpecialEffectData TileEffect
    {
        get => tileEffect;
        set => tileEffect = value;
    }
    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

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

    public void ApplyVisual(MatchTile prefab)
    {
        icon.sprite = prefab.Icon.sprite;
        animator.runtimeAnimatorController = prefab.Animator.runtimeAnimatorController;
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
        throw new System.NotImplementedException();
    }
}
