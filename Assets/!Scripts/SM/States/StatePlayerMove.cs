using UnityEngine;

public class StatePlayerMove : State<PlayerController>, IPhysicsState 
{
    public StatePlayerMove(PlayerController character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void LogicUpdate() { }

    public void PhysicsUpdate()
    {
        _character.transform.Rotate(Vector3.up, _character._rotation.action.ReadValue<float>() * _character._rotSpeed, Space.World);
        Vector2 input = _character._move.action.ReadValue<Vector2>();
        float speedMod = _character._shift.action.IsPressed() ? 1.5f : 1f;

        Vector3 dir = _character.transform.forward * input.y + _character.transform.right * input.x;
        if (dir.magnitude > 1f) dir.Normalize();

        Vector3 targetVelocity = dir * (_character._moveSpeed * speedMod);
        _character._rb.linearVelocity = new Vector3(targetVelocity.x, _character._rb.linearVelocity.y, targetVelocity.z);
    }
}