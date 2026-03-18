public abstract class StateEnemyAttack<T> : State<T> where T : Enemy
{
    public StateEnemyAttack(T character, StateMachine stateMachine) : base(character, stateMachine) { }
    
    public override void Enter()
    {
        _character._agent.isStopped = true;
        _character._animator.SetBool("IsAttack", true);
    }

    public override void Exit() => _character._animator.SetBool("IsAttack", false);

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if (eventType == AnimationEventType.AttackEnd)
        {
            _SM.ChangeState(_character._chaseState);
        }
    }
}