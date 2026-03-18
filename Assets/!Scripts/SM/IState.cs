// ISP: Интерфейс состояния стал чище. События передаются через Enum.
public interface IState
{
    void Enter();
    void Exit();
    void LogicUpdate();
    void PhysicsUpdate();
    void OnAnimationEvent(AnimationEventType eventType); 
}