using System;

public class CooldownTimer
{
    private readonly float _cooldownTime;
    private float _currentTimer;
    private bool _isActive = false;
    
    public event Action<float> OnCooldownProgress;
    public bool IsReady => !_isActive;

    public CooldownTimer(float cooldownTime)
    {
        _cooldownTime = cooldownTime;
    }

    public void StartCooldown()
    {
        _currentTimer = _cooldownTime;
        _isActive = true;
        OnCooldownProgress?.Invoke(1f);
    }

    // Вызываем вручную из Update контроллера
    public void Tick(float deltaTime)
    {
        if (!_isActive) return;

        _currentTimer -= deltaTime;
        if (_currentTimer <= 0f)
        {
            _currentTimer = 0f;
            _isActive = false;
        }
        
        OnCooldownProgress?.Invoke(_currentTimer / _cooldownTime);
    }
}