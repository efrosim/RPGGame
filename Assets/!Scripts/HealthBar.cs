using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Tooltip("Ссылка на зеленую полоску (Image)")]
    [SerializeField] private Image _fillImage;
    
    [Tooltip("Ссылка на персонажа. Если пусто, попытается найти на родительском объекте")]
    [SerializeField] private Character _character;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        
        if (_character == null)
        {
            _character = GetComponentInParent<Character>();
        }
    }

    private void OnEnable()
    {
        if (_character != null)
        {
            // Подписываемся на событие изменения здоровья
            _character.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void OnDisable()
    {
        if (_character != null)
        {
            // Отписываемся от события, чтобы избежать утечек памяти
            _character.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void Start()
    {
        if (_character != null)
        {
            // Устанавливаем начальное значение
            UpdateHealthBar(_character.GetHealthNormalized());
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
        // Эффект Billboard: заставляем Canvas всегда смотреть прямо в камеру
        if (_mainCamera != null)
        {
            transform.LookAt(transform.position + _mainCamera.transform.forward);
        }
    }
}