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

    public void LogicUpdate() => _curState?.LogicUpdate();
    
    public void PhysicsUpdate()
    {
        if (_curState is IPhysicsState physicsState)
            physicsState.PhysicsUpdate();
    }

    public void OnAnimationEvent(AnimationEventType eventType)
    {
        if (_curState is IAnimationState animState)
            animState.OnAnimationEvent(eventType);
    }

    // Пробрасываем получение урона в текущее состояние
    public void OnHit(int dmg, DamageType type) => _curState?.OnHit(dmg, type);
}