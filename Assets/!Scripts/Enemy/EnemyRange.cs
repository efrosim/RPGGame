using UnityEngine;
using UnityEngine.AI;

public class EnemyRange : Enemy
{
    [Header("Range Attack")]
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;

    protected void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _SM = new StateMachine();

        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _attackState = new StateEnemyRangeAttack(this, _SM);

        _SM.Init(_idleState);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void EventHandler(AnimEnums state)
    {
        base.EventHandler(state);
    }

    public int GetDmg()
    {
        return _dmg;
    }

    public void RangeAttackSheelCreate()
    {
        GameObject shell = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        shell.GetComponent<Shell>()._ownedBy = this;
    }
}
