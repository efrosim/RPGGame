using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public float _attackRange;
    public float _idleRange;

    public NavMeshAgent _agent;
    protected int _reloadTime;
    protected bool _readyForAttack;
    protected bool _isAttack;

    [Header("State Machine")]
    private StateMachine _SM;
    public StateEnemyChase _chaseState;
    public StateEnemyIdle _idleState;
    public StateEnemyMeleeAttack _meleeAttackState;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _SM = new StateMachine();

        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _meleeAttackState = new StateEnemyMeleeAttack(this, _SM);

        _SM.Init(_idleState);
    }

    private void FixedUpdate()
    {
        _SM._curState.LogicUpdate();
        _SM._curState.Update();
    }

    public void EventHandler(AnimEnums state)
    {
        _SM._curState.EventHandler(state);
    }

    protected virtual void GetHit(int dmg)
    {
        _HP -= dmg;
 //       if (_HP < 1) Dead();
    }

    //protected override void Attack()
    //{
    //    if(_readyForAttack)
    //    {
    //        _isAttack = true; //Выключить в анимации, в теории флаги все туда можно засунуть
    //        _readyForAttack = false;
    //        StartCoroutine(Timer(_reloadTime, _readyForAttack));
    //        Debug.Log("EnemyAttack");
    //    }

    //    if(!_isAttack)
    //    {
    //        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > _attackRange) ChangeState(EnemyState.chase);
    //    }
    //}
}
