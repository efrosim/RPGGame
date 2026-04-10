using UnityEngine;

public class StatePlayerMove : State<PlayerView>, IPhysicsState 
{
    private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;

    public StatePlayerMove(PlayerView character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character._animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void LogicUpdate() { }

    public void PhysicsUpdate()
    {
        _character.transform.Rotate(Vector3.up, _character.rotationAction.action.ReadValue<float>() * 2f, Space.World);
        Vector2 input = _character.moveAction.action.ReadValue<Vector2>();
        float speedMod = _character.shiftAction.action.IsPressed() ? 1.5f : 1f;

        Vector3 dir = _character.transform.forward * input.y + _character.transform.right * input.x;
        if (dir.magnitude > 1f) dir.Normalize();

        Vector3 targetVelocity = dir * (5f * speedMod);
        _character.Rb.linearVelocity = new Vector3(targetVelocity.x, _character.Rb.linearVelocity.y, targetVelocity.z);

        // Передаем скорость в Blend Tree Аниматора
        _character._animator.SetFloat(SpeedHash, dir.magnitude * speedMod);
    }
}