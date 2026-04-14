public abstract class State<T> : IState where T : class 
{
    protected T _character;
    protected StateMachine _SM;

    public State(T character, StateMachine stateMachine)
    {
        _character = character;
        _SM = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
    
    // По умолчанию состояния игнорируют урон (не прерываются). 
    // Те состояния, которые должны прерываться, переопределят этот метод.
    public virtual void OnHit(int dmg, DamageType type) { }
}