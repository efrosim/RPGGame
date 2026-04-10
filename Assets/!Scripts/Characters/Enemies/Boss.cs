using System;
using UnityEngine;

public enum BossElement
{
    Fire, Ice, Earth, Aether
}

public class Boss : Enemy
{
    public BossElement Element { get; private set; }
    
    // Boss can use generic IWeapon but maybe has unique ones
    public IWeapon Weapon { get; private set; }

    [SerializeField] private Material _fireMat, _iceMat, _earthMat, _aetherMat;
    [SerializeField] private Renderer _renderer;
    
    [Header("Boss Actions Config")]
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

        // Assign random element (Lab 6 extra: 4 elements)
        Element = (BossElement)UnityEngine.Random.Range(0, 4);
        ApplyElementVisuals();

        // States: Idle, Chase, Attack, HeavyAttack, Teleport, Shield, Summon
        AddState(new StateBossIdle(this, _SM));
        AddState(new StateBossChase(this, _SM));
        AddState(new StateBossAttack(this, _SM));
        AddState(new StateBossHeavyAttack(this, _SM));
        AddState(new StateBossTeleport(this, _SM));
        AddState(new StateBossShield(this, _SM));
        AddState(new StateBossSummon(this, _SM));

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

    public override void TransitionToAttackState()
    {
        // Decide which attack based on cooldowns and distance
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
        
        // If neutral in peaceful mode, and we are in idle, we switch to chase
        if (_SM._curState is StateBossIdle && GameController.IsPeacefulMode)
        {
            if (Target == null) Target = Scanner.Scan();
            ChangeState<StateBossChase>();
        }
        
        // Maybe teleport away if taking too much damage
        if (Time.time - LastTeleportTime > TeleportCooldown && UnityEngine.Random.value > 0.7f)
        {
            ChangeState<StateBossTeleport>();
        }
    }
}
