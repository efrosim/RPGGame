using UnityEngine;

public class StatePlayerMove : State
{
    public StatePlayerMove(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
    }
    public override void Exit()
    {

    }
    public override void Update()
    {

    }
}
