using System;
using UnityEngine;

public enum BossElement
{
    Fire, Ice, Earth, Aether
}

public class Boss : Enemy
{
    public BossElement Element { get; private set; }
    public bool IsMeleeWeapon { get; private set; }
    public IBossAttackEffectStrategy AttackEffect { get; private set; }

    [SerializeField] private Material _fireMat, _iceMat, _earthMat, _aetherMat;
    [SerializeField] private Renderer _renderer;[Header("Boss Actions Config")]
    public float HeavyAttackRange = 3f;
    public float TeleportCooldown = 15f;
    public float ShieldCooldown = 20f;
    public float SummonCooldown = 25f;

    public float LastTeleportTime { get; set; }
    public float LastShieldTime { get; set; }
    public float LastSummonTime { get; set; }

    protected override void Awake()
    {
        base.Awake();

        Element = (BossElement)UnityEngine.Random.Range(0, 4);
        IsMeleeWeapon = UnityEngine.Random.value > 0.5f; // Случайный тип оружия
        
        ApplyElementVisuals();
        
        // Получаем уникальную стратегию эффекта
        AttackEffect = BossEffectFactory.GetEffect(Element, IsMeleeWeapon);

        AddState(new StateBossIdle(this, _SM));
        AddState(new StateBossChase(this, _SM));
        AddState(new StateBossAttack(this, _SM));
        AddState(new StateBossHeavyAttack(this, _SM));
        AddState(new StateBossTeleport(this, _SM));
        AddState(new StateBossShield(this, _SM));
        AddState(new StateBossSummon(this, _SM));
        AddState(new StateEnemyHit(this, _SM));

        ChangeState<StateBossIdle>();
    }

    private void ApplyElementVisuals()
    {
        if (_renderer == null) return;
        switch (Element)
        {
            case BossElement.Fire: _renderer.material = _fireMat; break;
            case BossElement.Ice: _renderer.material = _iceMat; break;
            case BossElement.Earth: _renderer.material = _earthMat; break;
            case BossElement.Aether: _renderer.material = _aetherMat; break;
        }
    }

    public void PlayAttackEffect()
    {
        AttackEffect?.PlayEffect(transform);
    }

    public override void TransitionToAttackState()
    {
        if (Time.time - LastShieldTime > ShieldCooldown && HP < MaxHP * 0.5f)
        {
            ChangeState<StateBossShield>();
        }
        else if (Time.time - LastSummonTime > SummonCooldown)
        {
            ChangeState<StateBossSummon>();
        }
        else if (Target != null && Vector3.Distance(transform.position, Target.TargetPosition) <= HeavyAttackRange && UnityEngine.Random.value > 0.5f)
        {
            ChangeState<StateBossHeavyAttack>();
        }
        else
        {
            ChangeState<StateBossAttack>();
        }
    }

    protected override void OnHitReceived(int dmg, DamageType type)
    {
        base.OnHitReceived(dmg, type); 
        
        if (Time.time - LastTeleportTime > TeleportCooldown && UnityEngine.Random.value > 0.7f)
        {
            ChangeState<StateBossTeleport>();
        }
    }
}