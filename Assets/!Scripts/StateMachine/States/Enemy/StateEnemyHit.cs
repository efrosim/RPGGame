using UnityEngine;

public class StateEnemyHit : State<Enemy>
{
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private const float CrossFadeDuration = 0.1f;
    
    private float _stunTimer;
    private const float StunDuration = 0.5f; 

    public StateEnemyHit(Enemy character, StateMachine stateMachine) : base(character, stateMachine) { }

    public override void Enter()
    {
        _character.Agent.isStopped = true; 
        _character._animator.CrossFadeInFixedTime(HitHash, CrossFadeDuration);
        _stunTimer = 0f;

        // В мирном режиме обычные мобы игнорируют обидчика. Босс - агрится.
        if (!GameController.IsPeacefulMode || _character is Boss)
        {
            if (_character.Target == null)
                _character.Target = _character.Scanner.Scan();
        }
    }

    public override void LogicUpdate()
    {
        _stunTimer += Time.deltaTime;
        if (_stunTimer >= StunDuration)
        {
            if (GameController.IsPeacefulMode && !(_character is Boss))
            {
                // Обычный моб в мирном режиме не агрится
                _character.ChangeState<StateEnemyIdle>();
            }
            else
            {
                // Агрессивный режим или это Босс (он агрится после первого удара)
                _character.ChangeState<StateEnemyChase>();
            }
        }
    }
    
    public override void OnHit(int dmg, DamageType type)
    {
        _stunTimer = 0f; // Если бьют во время стана, таймер стана сбрасывается
    }
}