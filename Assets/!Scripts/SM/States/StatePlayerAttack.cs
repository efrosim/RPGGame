public abstract class StatePlayerAttack : State<PlayerController>, IAnimationState
{
    public StatePlayerAttack(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter() => _character._animator.SetBool("IsAttack", true);
    public override void Exit() => _character._animator.SetBool("IsAttack", false);

    public virtual void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd) 
            _character.ChangeState<StatePlayerMove>();
    }
}