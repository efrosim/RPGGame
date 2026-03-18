using UnityEngine;

// SRP: Отдельный компонент для сенсорики
public class TargetScanner : MonoBehaviour
{
    public float _detectionRadius = 15f;
    public LayerMask _targetLayer;
    
    // Оптимизация: NonAlloc массив
    private Collider[] _hits = new Collider[5];

    public ITargetable Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _hits, _targetLayer);
        for (int i = 0; i < count; i++)
        {
            if (_hits[i].TryGetComponent(out ITargetable target) && target.IsValidTarget)
            {
                return target;
            }
        }
        return null;
    }
}