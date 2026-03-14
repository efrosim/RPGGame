using UnityEngine;

public class StateEnemyDead : State
{
    private new Enemy _character;
    public StateEnemyDead(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (Enemy)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = true;
        _character._agent.enabled = false;
        _character.OnDead();
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
