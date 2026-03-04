using UnityEngine;

public class StateEnemyMeleeAttack : State
{
    private new Enemy _character;
    public StateEnemyMeleeAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
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

    }
}
