using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("Настройки цели")][Tooltip("Объект, за которым следит камера (создайте пустой объект CameraPivot внутри игрока)")]
    [SerializeField] private Transform _target;[Header("Настройки дистанции")]
    [SerializeField] private float _defaultDistance = 5f; // Стандартное расстояние до игрока
    [SerializeField] private float _minDistance = 1f;     // Минимальное расстояние при приближении
    [SerializeField] private float _smoothSpeed = 10f;    // Скорость сглаживания движения камеры

    [Header("Настройки столкновений")][Tooltip("Слои, которые считаются стенами. ОБЯЗАТЕЛЬНО исключите слой самого игрока!")][SerializeField] private LayerMask _obstacleLayers; 
    [SerializeField] private float _cameraRadius = 0.3f;  // Радиус "шарика" камеры

    private Vector3 _localDirection; // Направление относительно ПОВОРОТА игрока
    private float _currentDistance;

    private void Start()
    {
        // 1. Вычисляем направление в мировых координатах
        Vector3 worldDirection = (transform.position - _target.position).normalized;
        
        // 2. Переводим это направление в ЛОКАЛЬНЫЕ координаты цели.
        _localDirection = _target.InverseTransformDirection(worldDirection);
        
        _currentDistance = _defaultDistance;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        // Переводим локальное направление обратно в мировое, с учетом поворота игрока
        Vector3 currentWorldDirection = _target.TransformDirection(_localDirection);

        float targetDistance = _defaultDistance;

        // Пускаем луч от цели в сторону камеры по новому направлению
        if (Physics.SphereCast(_target.position, _cameraRadius, currentWorldDirection, out RaycastHit hit, _defaultDistance, _obstacleLayers))
        {
            // Если попали в стену, приближаем камеру
            targetDistance = Mathf.Clamp(hit.distance, _minDistance, _defaultDistance);
        }

        // Плавно меняем текущую дистанцию до нужной
        _currentDistance = Mathf.Lerp(_currentDistance, targetDistance, Time.deltaTime * _smoothSpeed);

        // Применяем позицию и заставляем камеру смотреть на цель
        transform.position = _target.position + currentWorldDirection * _currentDistance;
        transform.LookAt(_target);
    }
}