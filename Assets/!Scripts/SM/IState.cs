public interface IState
{
    void Enter();
    void Exit();
    void LogicUpdate();
}

public interface IPhysicsState
{
    void PhysicsUpdate();
}

public interface IAnimationState
{
    void OnAnimationEvent(AnimationEventType eventType);
}