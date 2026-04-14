using UnityEngine;

public class StateBossIdle : State<Boss>
{
    private float _scanTimer;
    public StateBossIdle(Boss character, StateMachine sm) : base(character, sm) { }

    public override void Enter() { _character.Agent.isStopped = true; }

    public override void LogicUpdate()
    {
        _scanTimer += Time.deltaTime;
        if (!GameController.IsPeacefulMode || _character.Target != null)
        {
            if (_scanTimer >= 0.2f) 
            {
                _character.Target = _character.Scanner.Scan();
                _scanTimer = 0f;
            }
            if (_character.Target != null) _character.ChangeState<StateBossChase>();
        }
    }

    public override void OnHit(int dmg, DamageType type) => _character.ChangeState<StateEnemyHit>();
}

public class StateBossChase : State<Boss>, IPhysicsState
{
    private static readonly int ChaseHash = Animator.StringToHash("Chase");
    public StateBossChase(Boss character, StateMachine sm) : base(character, sm) { }

    public override void Enter() 
    {
        _character.Agent.isStopped = false;
        _character._animator?.CrossFadeInFixedTime(ChaseHash, 0.1f);
    }

    public override void LogicUpdate()
    {
        if (_character.Target == null || !_character.Target.IsValidTarget) 
        {
            _character.ChangeState<StateBossIdle>();
            return;
        }

        float dist = Vector3.Distance(_character.transform.position, _character.Target.TargetPosition);
        if (dist <= _character._attackRange) _character.TransitionToAttackState();
    }

    public void PhysicsUpdate()
    {
        if (_character.Target != null) _character.Agent.destination = _character.Target.TargetPosition;
    }

    public override void OnHit(int dmg, DamageType type) => _character.ChangeState<StateEnemyHit>();
}

// Атаки теперь ждут Animation Event и НЕ прерываются уроном (OnHit не переопределен)
public class StateBossAttack : State<Boss>, IAnimationState
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    public StateBossAttack(Boss character, StateMachine sm) : base(character, sm) { }
    
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character._animator?.CrossFadeInFixedTime(AttackHash, 0.1f);
        _character.PlayAttackEffect();
    }

    public void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd) _character.ChangeState<StateBossChase>();
    }
}

public class StateBossHeavyAttack : State<Boss>, IAnimationState
{
    private static readonly int HeavyAttackHash = Animator.StringToHash("HeavyAttack");
    public StateBossHeavyAttack(Boss character, StateMachine sm) : base(character, sm) { }
    
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character._animator?.CrossFadeInFixedTime(HeavyAttackHash, 0.1f);
        _character.PlayAttackEffect();
    }

    public void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd) _character.ChangeState<StateBossChase>();
    }
}

public class StateBossTeleport : State<Boss>
{
    private float _timer;
    private static readonly int TeleportHash = Animator.StringToHash("Teleport");
    public StateBossTeleport(Boss character, StateMachine sm) : base(character, sm) { }
    
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character.LastTeleportTime = Time.time;
        _character._animator?.CrossFadeInFixedTime(TeleportHash, 0.1f);

        if (_character.Target != null)
        {
            Vector3 newPos = _character.Target.TargetPosition - (_character.Target.TargetPosition - _character.transform.position).normalized * 5f;
            if (UnityEngine.AI.NavMesh.SamplePosition(newPos, out var hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
                _character.Agent.Warp(hit.position);
        }
        _timer = 0.5f;
    }
    public override void LogicUpdate() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) _character.ChangeState<StateBossChase>();
    }
}

public class StateBossShield : State<Boss>
{
    private float _timer;
    private static readonly int ShieldHash = Animator.StringToHash("Shield");
    public StateBossShield(Boss character, StateMachine sm) : base(character, sm) { }
    
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character.LastShieldTime = Time.time;
        _character._animator?.CrossFadeInFixedTime(ShieldHash, 0.1f);
        _timer = 3f; 
    }
    public override void LogicUpdate() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) _character.ChangeState<StateBossChase>();
    }
}

public class StateBossSummon : State<Boss>
{
    private float _timer;
    private static readonly int SummonHash = Animator.StringToHash("Summon");
    public StateBossSummon(Boss character, StateMachine sm) : base(character, sm) { }
    
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character.LastSummonTime = Time.time;
        _character._animator?.CrossFadeInFixedTime(SummonHash, 0.1f);
        _timer = 1.5f;
    }
    public override void LogicUpdate() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) _character.ChangeState<StateBossChase>();
    }
}