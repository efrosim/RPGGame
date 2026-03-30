using UnityEngine;

public class StateEnemyDead : State<Enemy>
{
    private static readonly int DeadHash = Animator.StringToHash("Dead");
    private const float CrossFadeDuration = 0.1f;

    public StateEnemyDead(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character.Agent.isStopped = true;
        _character.Agent.enabled = false;
        _character._animator.CrossFadeInFixedTime(DeadHash, CrossFadeDuration);
    }
}