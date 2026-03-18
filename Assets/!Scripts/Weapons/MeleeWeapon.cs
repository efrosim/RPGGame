using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public int _dmg = 10;[SerializeField] private Vector3 _hitCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float _hitOffset = 1f;

    [SerializeField] private int _maxTargets = 10;
    private Collider[] _hitColliders;
    
    private void Awake()
    {
        _hitColliders = new Collider[_maxTargets];
    }
    
    public void Use() => DealDamage();
    
    public void DealDamage()
    {
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        int count = Physics.OverlapBoxNonAlloc(hitCenter, _hitCube / 2, _hitColliders, transform.rotation);
        
        for (int i = 0; i < count; i++)
        {
            if (_hitColliders[i].gameObject == this.gameObject) continue;
            
            if (_hitColliders[i].TryGetComponent(out IHittable target))
            {
                target.GetHit(_dmg, DamageType.Melee);
            }
        }
    }
}