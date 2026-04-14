using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public int _dmg = 10;
    [SerializeField] private Vector3 _hitCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float _hitOffset = 1f;

    [SerializeField] private int _maxTargets = 20;
    private Collider[] _hitColliders;

    private IHittable _owner;

    private void Awake()
    {
        _hitColliders = new Collider[_maxTargets];
        _owner = GetComponentInParent<IHittable>();
    }

    public virtual void Use() => DealDamage();

    public virtual void DealDamage()
    {
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        int count = Physics.OverlapBoxNonAlloc(hitCenter, _hitCube / 2, _hitColliders, transform.rotation);

        for (int i = 0; i < count; i++)
        {
            if (_hitColliders[i].TryGetComponent(out IHittable target))
            {
                if (target == _owner) continue;
                target.GetHit(_dmg, DamageType.Melee);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f); 
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        Gizmos.matrix = Matrix4x4.TRS(hitCenter, transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, _hitCube);
    }
}