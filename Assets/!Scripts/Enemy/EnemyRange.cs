using UnityEngine;

public class EnemyRange : Enemy
{
    [Header("Range Attack")]
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;

    protected override void Awake()
    {
        base.Awake();
        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _attackState = new StateEnemyRangeAttack(this, _SM);
        _deadState = new StateEnemyDead(this, _SM);

        _SM.Init(_idleState);
    }

    public void RangeAttackSheelCreate()
    {
        GameObject shell = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        shell.GetComponent<Shell>().SetDamage(_dmg);
    }
}