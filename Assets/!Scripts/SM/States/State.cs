using UnityEngine;

public abstract class State
{
    protected Character _character;
    protected StateMachine _SM;

    public virtual void Enter()
    {
        
    }
    public virtual void Exit()
    {

    }
    public virtual void Update()
    {

    }
}
