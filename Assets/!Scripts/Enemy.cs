using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    idle,
    chase,
    attack,
    dead
}

public class Enemy : Character
{
    //Потенциально абстрактный класс
    [SerializeField] protected EnemyState _curState;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _chaseRange;
    [SerializeField] protected float _idleRange;

    protected NavMeshAgent _agent;
    protected int _reloadTime;
    protected bool _readyForAttack;
    protected bool _isAttack;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void ChangeState(EnemyState newstate)
    {
        _curState = newstate;
    }

    private void FixedUpdate()
    {
        switch (_curState)
        {
            default:
            case EnemyState.idle:
                break;
            case EnemyState.chase:
                Chase();
                break;
            case EnemyState.attack:
                //Attack();
                break;
        }
    }
    
    protected virtual void GetHit(int dmg)
    {
        _HP -= dmg;
 //       if (_HP < 1) Dead();
    }

    protected virtual void Chase()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > _idleRange)
        {
            ChangeState(EnemyState.idle);
            _agent.isStopped = true;
            return;
        }

        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < _attackRange)
        {
            ChangeState(EnemyState.attack);
            _agent.isStopped = true;
            return;
        }

        _agent.destination = PlayerController.Instance.transform.position;
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

    protected IEnumerator Timer(int time, bool flag)
    {
        yield return new WaitForSeconds(time);
        flag = true;
    }
}
