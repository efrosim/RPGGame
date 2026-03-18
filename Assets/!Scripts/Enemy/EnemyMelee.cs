using UnityEngine;

public class EnemyMelee : Enemy
{
    protected override void Awake()
    {
        base.Awake(); // LSP: Вызываем Awake из Enemy
        _chaseState = new StateEnemyChase(this, _SM);
        _idleState = new StateEnemyIdle(this, _SM);
        _attackState = new StateEnemyMeleeAttack(this, _SM);
        _deadState = new StateEnemyDead(this, _SM);

        _SM.Init(_idleState);
    }

    public void DealDmg()
    {
        if (Target != null && Target.IsValidTarget)
        {
            // Простая проверка дистанции для ближнего боя
            if (Vector3.Distance(transform.position, Target.TargetPosition) <= _attackRange)
            {
                if (Target is IHittable hittable)
                {
                    hittable.GetHit(_dmg, DamageType.Melee);
                }
            }
        }
    }
}