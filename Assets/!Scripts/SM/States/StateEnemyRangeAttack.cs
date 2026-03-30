using UnityEngine;

public class StateEnemyRangeAttack : StateEnemyAttack<EnemyRange>
{
    // Передаем хэш анимации дальнего боя
    protected override int AttackHash => Animator.StringToHash("RangeAttack");

    public StateEnemyRangeAttack(EnemyRange character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if(eventType == AnimationEventType.DealDamage) 
            _character.Range.Use();
        base.OnAnimationEvent(eventType);
    }
}