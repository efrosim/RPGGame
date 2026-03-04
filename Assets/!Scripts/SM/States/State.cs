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

    }
    public virtual void Exit()
    {

    }
    public virtual void EventHandler(AnimEnums animstate)
    {

    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void Update()
    {

    }
}
