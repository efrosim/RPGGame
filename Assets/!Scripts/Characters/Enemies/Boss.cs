using System;
using System.Collections.Generic;
using UnityEngine;

public enum BossElement
{
    Fire, Ice, Earth, Aether
}

public class Boss : Enemy
{
    [Header("Weapons")][SerializeField] private MeleeWeapon _meleeWeapon;
    [SerializeField] private RangeWeapon _rangeWeapon;

    // Свойство, которое возвращает нужное оружие в зависимости от текущей стихии
    public IWeapon CurrentWeapon => IsMeleeWeapon ? (IWeapon)_meleeWeapon : (IWeapon)_rangeWeapon;
    public BossElement Element { get; private set; }
    public bool IsMeleeWeapon { get; private set; }
    public IBossAttackEffectStrategy AttackEffect { get; private set; }

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

    [Header("Summoning")]
    [SerializeField] private Enemy[] _minionPrefabs;
    [SerializeField] private float _summonRadius = 4f;

    public float AttackSpeedMultiplier => HP < MaxHP * 0.5f ? 1.5f : 1.0f;

    private List<BossElement> _availableElements;
    private float _nextElementThreshold = 0.75f; // 75%, 50%, 25%

    protected override void Awake()
    {
        base.Awake();

        // Инициализируем список доступных стихий
        _availableElements = new List<BossElement> { BossElement.Fire, BossElement.Ice, BossElement.Earth, BossElement.Aether };
        
        // Выбираем первую случайную стихию
        SwitchToRandomElement();

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

    private void SwitchToRandomElement()
    {
        if (_availableElements.Count == 0) return;

        int idx = UnityEngine.Random.Range(0, _availableElements.Count);
        Element = _availableElements[idx];
        _availableElements.RemoveAt(idx); 

        IsMeleeWeapon = UnityEngine.Random.value > 0.5f;
        AttackEffect = BossEffectFactory.GetEffect(Element, IsMeleeWeapon);
        ApplyElementVisuals();

        if (_meleeWeapon != null) _meleeWeapon.SetElement(Element);
        if (_rangeWeapon != null) _rangeWeapon.SetElement(Element);

        Debug.Log($"✨ [Boss] Смена стихии на {Element}! (Здесь срабатывают партиклы)");
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
            ChangeState<StateBossShield>();
        else if (Time.time - LastSummonTime > SummonCooldown)
            ChangeState<StateBossSummon>();
        else if (Target != null && Vector3.Distance(transform.position, Target.TargetPosition) <= HeavyAttackRange && UnityEngine.Random.value > 0.5f)
            ChangeState<StateBossHeavyAttack>();
        else
            ChangeState<StateBossAttack>();
    }

    public override void TransitionToChaseState() => ChangeState<StateBossChase>();

    public override void TransitionToIdleState() => ChangeState<StateBossIdle>();

    public override void GetHit(int dmg, DamageType type)
    {
        if (_SM._curState is StateBossShield)
        {
            Debug.Log("🛡️ [Boss] Удар полностью заблокирован щитом!");
            return;
        }
        base.GetHit(dmg, type);
    }

    public void SummonMinions()
    {
        if (_minionPrefabs == null || _minionPrefabs.Length == 0) return;

        IWeaponFactory weaponFactory = null;
        var scope = UnityEngine.Object.FindAnyObjectByType<VContainer.Unity.LifetimeScope>();
        if (scope != null && scope.Container != null)
        {
            weaponFactory = scope.Container.Resolve<IWeaponFactory>();
        }

        int count = 2;
        for (int i = 0; i < count; i++)
        {
            Enemy prefab = _minionPrefabs[UnityEngine.Random.Range(0, _minionPrefabs.Length)];
            Vector3 randomDir = UnityEngine.Random.insideUnitSphere * _summonRadius;
            randomDir.y = 0;
            Vector3 spawnPos = transform.position + randomDir;

            if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out var hit, _summonRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                Enemy spawned = Instantiate(prefab, hit.position, Quaternion.identity);
                spawned.SetDynamicId($"{spawned.gameObject.name}_{System.Guid.NewGuid().ToString().Substring(0, 8)}");

                if (weaponFactory != null)
                {
                    IWeapon weapon = weaponFactory.EquipRandomWeapon(spawned);
                    spawned.InitWeapon(weapon);
                }
            }
        }
    }

    protected override void OnHitReceived(int dmg, DamageType type)
    {
        base.OnHitReceived(dmg, type); 
        
        // Проверка на смену стихии
        float hpPct = (float)HP / MaxHP;
        if (hpPct <= _nextElementThreshold && _availableElements.Count > 0)
        {
            SwitchToRandomElement();
            _nextElementThreshold -= 0.25f; // Следующий порог
        }

        if (Time.time - LastTeleportTime > TeleportCooldown && UnityEngine.Random.value > 0.7f)
        {
            ChangeState<StateBossTeleport>();
        }
    }
}