using System;
using UnityEngine;

public class CooldownTimer : MonoBehaviour
{
    public float _cooldownTime = 2f;
    private float _lastUseTime = -10f;
    private bool _isActive = false;
    
    public event Action<float> OnCooldownProgress;
    public bool IsReady => !_isActive;

    public void StartCooldown()
    {
        _lastUseTime = Time.time;
        _isActive = true;
        OnCooldownProgress?.Invoke(1f);
    }

    private void Update()
    {
        if (_isActive)
        {
            float progress = (Time.time - _lastUseTime) / _cooldownTime;
            if (progress >= 1f)
            {
                progress = 1f;
                _isActive = false;
            }
            OnCooldownProgress?.Invoke(1f - progress);
        }
    }
}