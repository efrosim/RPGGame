public interface IState
{
    void Enter();
    void Exit();
    void LogicUpdate();
    void OnHit(int dmg, DamageType type); 
}

public interface IPhysicsState
{
    void PhysicsUpdate();
}

public interface IAnimationState
{
    void OnAnimationEvent(AnimationEventType eventType);
}