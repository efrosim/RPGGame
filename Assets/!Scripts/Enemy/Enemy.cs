using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character
{
    public float _attackRange;
    public float _idleRange;

    public NavMeshAgent _agent;

    [Header("State Machine")]
    protected StateMachine _SM;
    public StateEnemyChase _chaseState;
    public StateEnemyIdle _idleState;
    public StateEnemyAttack _attackState;

    public float _attackCooldown = 1.3f; 
    [HideInInspector] public float _lastAttackTime;
    
    protected virtual void FixedUpdate()
    {
        if (IsDead) return; // Если мертв, ничего не делаем
        _SM._curState.LogicUpdate();
        _SM._curState.Update();
    }

    public virtual void EventHandler(AnimEnums state)
    {
        if (IsDead) return;
        _SM._curState.EventHandler(state);
    }

    public override void GetHit(int dmg, DamageType type)
    {
        if (IsDead) return;
        base.GetHit(dmg, type);

        if (_HP <= 0)
        {
            IsDead = true;
            _animator.SetTrigger("Dead"); // Анимация смерти
            _agent.isStopped = true;
            _agent.enabled = false; // Отключаем поиск пути
            Destroy(gameObject, 2.5f); // Удаляем труп через 2.5 секунды
        }
        else
        {
            _animator.SetTrigger("Hit"); // Микро-стан
            _lastAttackTime = Time.time; // Сбрасываем таймер атаки, чтобы моб не ударил сразу после стана
        }
    }
}