using UnityEngine;

public class StateBossIdle : State<Boss>
{
    private float _scanTimer;
    public StateBossIdle(Boss character, StateMachine sm) : base(character, sm) { }

    public override void Enter() 
    {
        _character.Agent.isStopped = true;
    }

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

            if (_character.Target != null)
            {
                _character.ChangeState<StateBossChase>();
            }
        }
    }
}

public class StateBossChase : State<Boss>, IPhysicsState
{
    public StateBossChase(Boss character, StateMachine sm) : base(character, sm) { }

    public override void Enter() 
    {
        _character.Agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        if (_character.Target == null || !_character.Target.IsValidTarget) 
        {
            _character.ChangeState<StateBossIdle>();
            return;
        }

        float dist = Vector3.Distance(_character.transform.position, _character.Target.TargetPosition);
        if (dist <= _character._attackRange)
        {
            _character.TransitionToAttackState();
        }
    }

    public void PhysicsUpdate()
    {
        if (_character.Target != null)
            _character.Agent.destination = _character.Target.TargetPosition;
    }
}

public class StateBossAttack : State<Boss>
{
    private float _timer;
    public StateBossAttack(Boss character, StateMachine sm) : base(character, sm) { }
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _timer = 1f; 
        _character._animator?.CrossFade("Attack", 0.1f);
        
        // Вызов уникального эффекта стратегии
        _character.PlayAttackEffect();
    }
    public override void LogicUpdate() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) _character.ChangeState<StateBossChase>();
    }
}

public class StateBossHeavyAttack : State<Boss>
{
    private float _timer;
    public StateBossHeavyAttack(Boss character, StateMachine sm) : base(character, sm) { }
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _timer = 2f; 
        _character._animator?.CrossFade("HeavyAttack", 0.1f);
        
        // Вызов уникального эффекта стратегии
        _character.PlayAttackEffect();
    }
    public override void LogicUpdate() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) _character.ChangeState<StateBossChase>();
    }
}

public class StateBossTeleport : State<Boss>
{
    private float _timer;
    public StateBossTeleport(Boss character, StateMachine sm) : base(character, sm) { }
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character.LastTeleportTime = Time.time;
        // Teleport behind player immediately
        if (_character.Target != null)
        {
            Vector3 newPos = _character.Target.TargetPosition - (_character.Target.TargetPosition - _character.transform.position).normalized * 5f;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(newPos, out hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
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
    public StateBossShield(Boss character, StateMachine sm) : base(character, sm) { }
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character.LastShieldTime = Time.time;
        _timer = 3f; // Keep shield up for 3 seconds
        // visual shield enable...
    }
    public override void LogicUpdate() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) _character.ChangeState<StateBossChase>();
    }
}

public class StateBossSummon : State<Boss>
{
    private float _timer;
    public StateBossSummon(Boss character, StateMachine sm) : base(character, sm) { }
    public override void Enter() { 
        _character.Agent.isStopped = true; 
        _character.LastSummonTime = Time.time;
        _timer = 1.5f;
        // Logic to spawn minions could trigger here via event or directly finding spawner
    }
    public override void LogicUpdate() {
        _timer -= Time.deltaTime;
        if (_timer <= 0) _character.ChangeState<StateBossChase>();
    }
}
