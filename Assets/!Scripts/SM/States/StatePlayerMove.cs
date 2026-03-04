using UnityEngine;

public class StatePlayerMove : State
{
    private new PlayerController _character;
    public StatePlayerMove(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (PlayerController)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
    }
    public override void Exit()
    {

    }
    public override void LogicUpdate()
    {
        if(_character._primeAttack.action.IsPressed())
        {
            _SM.ChangeState(_character._statePlayerMeleeAttack);
        }
    }
    public override void Update()
    {
        Rotation();
        Movement();
    }

    private void Rotation()
    {
        _character.transform.Rotate(Vector3.up, _character._rotation.action.ReadValue<float>() * _character._rotSpeed, Space.World);
    }
    private void Movement()
    {
        if (_character._move.action.IsPressed())
        {
            float speedMod = 1f;
            if (_character._shift.action.IsPressed())
                speedMod = 1.5f;

            Vector2 input = _character._move.action.ReadValue<Vector2>();
            Vector3 dir = input.y * speedMod * _character.transform.forward + input.x * speedMod * _character.transform.right;

            _character._rb.AddForce(dir * _character._moveSpeed, ForceMode.Force);
        }
    }
}
