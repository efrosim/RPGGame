using UnityEngine;

public class StateEnemyMeleeAttack : StateEnemyAttack<EnemyMelee>
{
    // Передаем хэш анимации ближнего боя
    protected override int AttackHash => Animator.StringToHash("MeleeAttack");

    public StateEnemyMeleeAttack(EnemyMelee character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void OnAnimationEvent(AnimationEventType eventType)
    {
        if(eventType == AnimationEventType.DealDamage) 
            _character.Melee.Use();
        base.OnAnimationEvent(eventType);
    }
}