using UnityEngine;

public class StateEnemyFlee : State<Enemy>, IPhysicsState
{
    private static readonly int FleeHash = Animator.StringToHash("Run"); // assuming Run animation exists, or use Chase
    private const float CrossFadeDuration = 0.1f;
    private float _fleeTimer;

    public StateEnemyFlee(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    private float _baseSpeed;

    public override void Enter() 
    {
        _character.Agent.isStopped = false;
        _baseSpeed = _character.Agent.speed;
        _character.Agent.speed = _baseSpeed * 1.5f; // Fast run
        _character._animator.CrossFadeInFixedTime(FleeHash, CrossFadeDuration);
        _fleeTimer = 0f;
        FindFleePoint();
    }

    public override void LogicUpdate()
    {
        _fleeTimer += Time.deltaTime;
        
        // Find new flee point every 3 seconds
        if (_fleeTimer > 3f)
        {
            _fleeTimer = 0f;
            FindFleePoint();
        }

        if (_character.Agent.remainingDistance < 1f)
        {
            FindFleePoint();
        }
    }

    public void PhysicsUpdate() { }

    private void FindFleePoint()
    {
        Vector3 playerPos = _character.Target != null ? _character.Target.TargetPosition : Vector3.zero;
        if (playerPos == Vector3.zero)
        {
            var player = Object.FindAnyObjectByType<PlayerView>();
            if (player != null) playerPos = player.transform.position;
        }

        Vector3 fleeDir = (_character.transform.position - playerPos).normalized;
        Vector3 fleePoint = _character.transform.position + fleeDir * 10f;
        
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(fleePoint, out hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
        {
            _character.Agent.SetDestination(hit.position);
        }
    }

    public override void Exit()
    {
        _character.Agent.speed = _baseSpeed; // Reset speed
    }
    
    public override void OnHit(int dmg, DamageType type)
    {
        if (_character.HP <= _character.MaxHP * 0.3f && !(_character is Boss))
            _character.ChangeState<StateEnemyFlee>();
        else
            _character.ChangeState<StateEnemyHit>();
    }
}
