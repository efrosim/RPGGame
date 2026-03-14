using UnityEngine;

public class StateEnemyAttack : State
{
    private new Enemy _character;
    public StateEnemyAttack(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        _character = (Enemy)character;
    }

    public override void Enter()
    {
        Debug.Log("Cur State: " + _SM._curState);
        _character._agent.isStopped = true;
        
        // Возвращаем Bool для того чтобы не происходило быстрое переключение
        _character._animator.SetBool("IsAttack", true); 
    }

    public override void Exit()
    {
        // Сбрасываем Bool, чтобы аниматор не зависал в атаке
        _character._animator.SetBool("IsAttack", false);
    }

    public override void EventHandler(AnimEnums animstate)
    {
        Debug.Log("Here");
        if (animstate == AnimEnums.AttackEnd) OnAttackEnd();
    }

    public override void LogicUpdate()
    {

    }

    private void OnAttackEnd()
    {
        _SM.ChangeState(_character._chaseState);
    }
}