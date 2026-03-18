using UnityEngine;

public class StateEnemyMeleeAttack : StateEnemyAttack<EnemyMelee>
{
    public StateEnemyMeleeAttack(EnemyMelee character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }


    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void EventHandler(AnimEnums animstate)
    {
        base.EventHandler(animstate);

        if(animstate == AnimEnums.DealDmg) OnDealDmg();
    }

    public override void LogicUpdate()
    {

    }

    private void OnDealDmg()
    {
        // _character здесь автоматически распознается как EnemyMelee!
        // Поэтому метод DealDmg() доступен без всяких кастов.
        if (Vector3.Distance(_character.transform.position, PlayerController.Instance.transform.position) < _character._attackRange)
        {
            _character.DealDmg();
        }
    }
}
