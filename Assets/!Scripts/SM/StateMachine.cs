public class StateMachine
{
    public IState _curState; // Заменили State на IState

    public void Init(IState startState)
    {
        _curState = startState;
        _curState.Enter();
    }

    public void ChangeState(IState newState)
    {
        _curState?.Exit(); // Добавили знак вопроса на всякий случай
        _curState = newState;
        _curState.Enter();
    }
}