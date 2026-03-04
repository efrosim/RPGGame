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
        // Читаем ввод каждый кадр (если кнопки не нажаты, будет Vector2.zero)
        Vector2 input = _character._move.action.ReadValue<Vector2>();
    
        float speedMod = 1f;
        if (_character._shift.action.IsPressed())
            speedMod = 1.5f;

        // Вычисляем направление движения относительно поворота персонажа
        Vector3 dir = _character.transform.forward * input.y + _character.transform.right * input.x;

        // Нормализуем вектор, чтобы при движении по диагонали скорость не была выше
        if (dir.magnitude > 1f)
        {
            dir.Normalize();
        }

        // Вычисляем итоговую целевую скорость
        Vector3 targetVelocity = dir * (_character._moveSpeed * speedMod);

        // Применяем скорость к Rigidbody. 
        // Важно: мы сохраняем текущую скорость по оси Y (_rb.velocity.y), чтобы гравитация и падение работали корректно!
        _character._rb.linearVelocity = new Vector3(targetVelocity.x, _character._rb.linearVelocity.y, targetVelocity.z);
    }
}
