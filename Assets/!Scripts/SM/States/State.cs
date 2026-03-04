using UnityEngine;

public abstract class State
{
    protected Character _character;
    protected StateMachine _SM;

    public State(Character character, StateMachine stateMachine)
    {
        _character = character;
        _SM = stateMachine;
    }

    public virtual void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
    }
    public virtual void Exit()
    {

    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void Update()
    {

    }
}
