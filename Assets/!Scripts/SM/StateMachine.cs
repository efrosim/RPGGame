public class StateMachine
{
    public IState _curState { get; private set; }

    public void Init(IState startState)
    {
        _curState = startState;
        _curState?.Enter();
    }

    public void ChangeState(IState newState)
    {
        _curState?.Exit();
        _curState = newState;
        _curState?.Enter();
    }
}