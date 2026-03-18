public abstract class State<T> : IState where T : Character 
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
    public virtual void PhysicsUpdate() { }
    public virtual void OnAnimationEvent(AnimationEventType eventType) { }
}