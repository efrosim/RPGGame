using UnityEngine;

public abstract class StatePlayerAttack : State<PlayerController>, IAnimationState
{
    protected abstract int AttackHash { get; }
    private const float CrossFadeDuration = 0.1f;

    public StatePlayerAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter() 
    {
        // Останавливаем игрока, чтобы он не скользил во время удара
        _character._rb.linearVelocity = Vector3.zero;
        _character._animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Exit() { }

    public virtual void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd) 
            _character.ChangeState<StatePlayerMove>();
    }
}