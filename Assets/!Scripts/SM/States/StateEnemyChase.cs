using UnityEngine;

public class StateEnemyChase : State<Enemy>, IPhysicsState
{
    public StateEnemyChase(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter() => _character._agent.isStopped = false;
    public override void Exit() => _character._animator.SetBool("IsChase", false);

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
            _character.TransitionToAttackState(); // OCP: Вызов абстрактного метода
    }
    
    public void PhysicsUpdate()
    {
        if (_character.Target == null) return;
        _character._agent.destination = _character.Target.TargetPosition;
        _character._animator.SetBool("IsChase", _character._agent.velocity.sqrMagnitude >= 0.01f);
    }
}