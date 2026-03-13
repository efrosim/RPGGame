using UnityEngine;

public class StateEnemyChase : State
{
    private new Enemy _character;
    public StateEnemyChase(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (Enemy)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = false;
        // Убрали принудительное включение анимации бега отсюда, будем проверять скорость в Update
    }

    public override void Exit()
    {
        _character._animator.SetBool("IsChase", false);
    }

    public override void EventHandler(AnimEnums animstate)
    {

    }
    
    public override void LogicUpdate()
    {
        float distanceToPlayer = Vector3.Distance(_character.transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer > _character._idleRange)
        {
            _SM.ChangeState(_character._idleState);
        }
        // Проверяем дистанцию и кулдаун
        else if (distanceToPlayer <= _character._attackRange && Time.time >= _character._lastAttackTime + _character._attackCooldown)
        {
            _SM.ChangeState(_character._attackState);
        }
    }
    
    public override void Update()
    {
        _character._agent.destination = PlayerController.Instance.transform.position;
        
        if (_character._agent.velocity.sqrMagnitude < 0.01f)
        {
            _character._animator.SetBool("IsChase", false);
        }
        else
        {
            _character._animator.SetBool("IsChase", true);
        }
    }
}