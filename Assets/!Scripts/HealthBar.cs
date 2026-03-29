using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [Tooltip("Объект, реализующий IHealth")]
    [SerializeField] private GameObject _healthSourceObj; 
    
    private IHealth _healthSource;
    private Camera _mainCamera; // Вернули ссылку на камеру

    private void Awake()
    {
        _mainCamera = Camera.main;
        
        // Получаем абстракцию IHealth
        if (_healthSourceObj != null)
            _healthSource = _healthSourceObj.GetComponent<IHealth>();
        else
            _healthSource = GetComponentInParent<IHealth>();
    }

    private void OnEnable()
    {
        if (_healthSource != null)
            _healthSource.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if (_healthSource != null) // ИСПРАВЛЕНО: используем _healthSource вместо _character
        {
            _healthSource.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void Start()
    {
        if (_healthSource != null) // ИСПРАВЛЕНО: используем _healthSource вместо _character
        {
            UpdateHealthBar(_healthSource.GetHealthNormalized());
        }
    }

    private void UpdateHealthBar(float normalizedHealth)
    {
        if (_fillImage != null)
        {
            _fillImage.fillAmount = normalizedHealth;
        }
    }

    private void LateUpdate()
    {
        if (_mainCamera != null)
        {
            transform.LookAt(transform.position + _mainCamera.transform.forward);
        }
    }
}