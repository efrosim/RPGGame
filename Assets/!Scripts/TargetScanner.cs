using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    public float _detectionRadius = 15f;
    public LayerMask _targetLayer;
    
    [SerializeField] private int _maxTargets = 5;
    private Collider[] _hits;

    private void Awake()
    {
        _hits = new Collider[_maxTargets];
    }
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