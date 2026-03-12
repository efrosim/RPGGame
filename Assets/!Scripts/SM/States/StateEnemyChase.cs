using UnityEngine;

public class StateEnemyChase : State
{
    private new Enemy _character;
    public StateEnemyChase(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (Enemy)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = false;
        _character._animator.SetBool("IsChase", true);
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
        if (Vector3.Distance(_character.transform.position, PlayerController.Instance.transform.position) > _character._idleRange)
        {
            _SM.ChangeState(_character._idleState);
        }

        if (Vector3.Distance(_character.transform.position, PlayerController.Instance.transform.position) < _character._attackRange)
        {
            _SM.ChangeState(_character._attackState);
        }
    }
    public override void Update()
    {
        _character._agent.destination = PlayerController.Instance.transform.position;
    }
}
