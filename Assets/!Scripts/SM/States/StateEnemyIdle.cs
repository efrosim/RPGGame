using UnityEngine;

public class StateEnemyIdle : State
{
    private new Enemy _character;
    public StateEnemyIdle(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (Enemy)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = true;
    }

    public override void Exit()
    {

    }

    public override void EventHandler(AnimEnums animstate)
    {

    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(_character.transform.position, PlayerController.Instance.transform.position) < _character._idleRange)
        {
            _SM.ChangeState(_character._chaseState);
        }
    }
}
