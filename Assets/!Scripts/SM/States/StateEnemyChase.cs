using UnityEngine;

public class StateEnemyChase : State<Enemy>, IPhysicsState
{
    private static readonly int ChaseHash = Animator.StringToHash("Chase");
    private const float CrossFadeDuration = 0.1f;

    public StateEnemyChase(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter() 
    {
        _character.Agent.isStopped = false;
        _character._animator.CrossFadeInFixedTime(ChaseHash, CrossFadeDuration);
    }

    public override void Exit() { }

    public override void LogicUpdate()
    {
        if (_character.Target == null || !_character.Target.IsValidTarget) 
        {
            _character.Target = null;
            _character.ChangeState<StateEnemyIdle>();
            return;
        }

        float distanceToTarget = Vector3.Distance(_character.transform.position, _character.Target.TargetPosition);

        if (distanceToTarget > _character._idleRange)
            _character.ChangeState<StateEnemyIdle>();
        else if (distanceToTarget <= _character._attackRange)
            _character.TransitionToAttackState(); 
    }
    
    public void PhysicsUpdate()
    {
        if (_character.Target == null) return;
        _character.Agent.destination = _character.Target.TargetPosition;
        // SetBool("IsChase") удален, так как стейт сам контролирует анимацию
    }
}