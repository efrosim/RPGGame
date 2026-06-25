using UnityEngine;

public class StatePlayerIdle : State<PlayerView>, IPhysicsState
{
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private const float CrossFadeDuration = 0.1f;

    public StatePlayerIdle(PlayerView character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character.Rb.linearVelocity = new Vector3(0f, _character.Rb.linearVelocity.y, 0f);
        _character._animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }

    public override void LogicUpdate()
    {
        Vector2 input = _character.moveAction.action.ReadValue<Vector2>();
        if (input.sqrMagnitude > 0.001f)
        {
            _SM.ChangeState(new StatePlayerMove(_character, _SM));
        }
    }

    public void PhysicsUpdate()
    {
        _character.transform.Rotate(Vector3.up, _character.rotationAction.action.ReadValue<float>() * 2f, Space.World);
        _character.Rb.linearVelocity = new Vector3(0f, _character.Rb.linearVelocity.y, 0f);
    }

    public override void OnHit(int dmg, DamageType type)
    {
        _SM.ChangeState(new StatePlayerHit(_character, _SM));
    }
}
