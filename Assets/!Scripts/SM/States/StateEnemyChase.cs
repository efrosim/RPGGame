using UnityEngine;

public class StateEnemyChase : State<Enemy>
{
    public StateEnemyChase(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter() => _character._agent.isStopped = false;
    public override void Exit() => _character._animator.SetBool("IsChase", false);

    public override void LogicUpdate()
    {
        if (_character.Target == null || !_character.Target.IsValidTarget) 
        {
            _character.Target = null;
            _SM.ChangeState(_character._idleState);
            return;
        }

        float distanceToTarget = Vector3.Distance(_character.transform.position, _character.Target.TargetPosition);

        if (distanceToTarget > _character._idleRange)
            _SM.ChangeState(_character._idleState);
        else if (distanceToTarget <= _character._attackRange)
            _SM.ChangeState(_character._attackState);
    }
    
    public override void PhysicsUpdate()
    {
        if (_character.Target == null) return;

        _character._agent.destination = _character.Target.TargetPosition;
        _character._animator.SetBool("IsChase", _character._agent.velocity.sqrMagnitude >= 0.01f);
    }
}