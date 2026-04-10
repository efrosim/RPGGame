using System;
using UnityEngine;

public class PlayerController : IDisposable
{
    private readonly PlayerModel _model;
    private readonly PlayerView _view;
    private readonly StateMachine _sm;

    public PlayerController(PlayerModel model, PlayerView view)
    {
        _model = model;
        _view = view;
        _sm = new StateMachine();

        _sm.ChangeState(new StatePlayerMove(_view, _sm)); 

        _view.OnMoveInputEvent += HandleMoveInput;
        _view.OnPrimeAttackEvent += HandlePrimeAttack;
        _view.OnSecondAttackEvent += HandleSecondAttack;
        _view.OnHitEvent += HandleHit;

        _model.OnDead += HandleDeath;
    }

    private void HandleMoveInput(Vector2 input, bool isRunning)
    {
        // Currently state logic handles it inside StatePlayerMove,
        // but we might need to route it. For now let StatePlayerMove read from View directly.
    }

    private void HandlePrimeAttack()
    {
        if (_sm._curState is StatePlayerMove)
            _sm.ChangeState(new StatePlayerMeleeAttack(_view, _sm));
    }

    private void HandleSecondAttack()
    {
        if (_sm._curState is StatePlayerMove && _model.MagicCooldown.IsReady)
        {
            _model.MagicCooldown.StartCooldown();
            _sm.ChangeState(new StatePlayerRangeAttack(_view, _sm));
        }
    }

    private void HandleHit(int dmg, DamageType type)
    {
        _model.Health -= dmg;
        if (_model.Health > 0)
        {
            _sm.ChangeState(new StatePlayerHit(_view, _sm));
        }
    }

    private void HandleDeath()
    {
        _view.DisableInput();
        _sm.ChangeState(new StatePlayerHit(_view, _sm)); // Or dead state
    }

    public void LogicUpdate()
    {
        _sm.LogicUpdate();
        _model.MagicCooldown.Tick(Time.deltaTime);
    }

    public void PhysicsUpdate()
    {
        _sm.PhysicsUpdate();
    }

    public void Dispose()
    {
        _view.OnMoveInputEvent -= HandleMoveInput;
        _view.OnPrimeAttackEvent -= HandlePrimeAttack;
        _view.OnSecondAttackEvent -= HandleSecondAttack;
        _view.OnHitEvent -= HandleHit;
        _model.OnDead -= HandleDeath;
    }
}