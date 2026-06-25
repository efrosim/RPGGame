using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [Tooltip("Объект, реализующий IHealth")]
    [SerializeField] private GameObject _healthSourceObj; 
    
    private IHealth _healthSource;
    private Camera _mainCamera; 
    private Canvas _parentCanvas;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _parentCanvas = GetComponentInParent<Canvas>();
        
        if (_healthSourceObj != null)
        {
            _healthSource = _healthSourceObj.GetComponent<IHealth>();
        }

        if (_healthSource == null)
        {
            _healthSource = FindHealthSourceInParents();
        }

        if (_healthSource == null)
        {
            Debug.LogWarning($"[HealthBar] Could not find any component implementing IHealth in parent hierarchy of '{gameObject.name}'", this);
        }
    }

    private IHealth FindHealthSourceInParents()
    {
        Transform current = transform;
        while (current != null)
        {
            var health = current.GetComponent<IHealth>();
            if (health != null)
            {
                return health;
            }
            current = current.parent;
        }
        return null;
    }

    private void OnEnable()
    {
        if (_healthSource != null)
            _healthSource.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if (_healthSource != null) 
        {
            _healthSource.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void Start()
    {
        if (_healthSource == null)
        {
            _healthSource = FindHealthSourceInParents();
            if (_healthSource != null)
            {
                _healthSource.OnHealthChanged += UpdateHealthBar;
            }
        }

        if (_healthSource != null) 
        {
            UpdateHealthBar(_healthSource.GetHealthNormalized());
        }
        else
        {
            UpdateHealthBar(0f); // Default to empty if no source found
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
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        if (_mainCamera != null)
        {
            if (_parentCanvas != null && _parentCanvas.renderMode != RenderMode.WorldSpace)
            {
                return;
            }
            transform.rotation = _mainCamera.transform.rotation;
        }
    }
}