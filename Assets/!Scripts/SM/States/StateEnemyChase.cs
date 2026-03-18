using UnityEngine;

public class StateEnemyChase : State<Enemy>
{
    public StateEnemyChase(Enemy character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = false;
    }

    public override void Exit()
    {
        _character._animator.SetBool("IsChase", false);
    }

    public override void EventHandler(AnimEnums animstate)
    {

    }
    
    public override void LogicUpdate()
    {
        float distanceToPlayer = Vector3.Distance(_character.transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer > _character._idleRange)
        {
            _SM.ChangeState(_character._idleState);
        }

        else if (distanceToPlayer <= _character._attackRange)
        {
            _SM.ChangeState(_character._attackState);
        }
    }
    
    public override void Update()
    {
        _character._agent.destination = PlayerController.Instance.transform.position;
        
        if (_character._agent.velocity.sqrMagnitude < 0.01f)
        {
            _character._animator.SetBool("IsChase", false);
        }
        else
        {
            _character._animator.SetBool("IsChase", true);
        }
    }
}