using UnityEngine;

public class StateEnemyDead : State<Enemy>
{
    public StateEnemyDead(Enemy character, StateMachine stateMachine) : base(character, stateMachine)
    {
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
