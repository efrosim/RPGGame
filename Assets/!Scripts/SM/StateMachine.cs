using UnityEngine;

public abstract class StateMachine
{
    public State _curState;

    public void Init(State startState)
    {
        _curState = startState;
        _curState.Enter();
    }

    public void ChangeState(State newState)
    {
        _curState.Exit();
        _curState = newState;
        _curState.Enter();
    }
}
