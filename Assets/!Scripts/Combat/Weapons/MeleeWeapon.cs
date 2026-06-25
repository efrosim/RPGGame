using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public int _dmg = 10;
    [SerializeField] private Vector3 _hitCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float _hitOffset = 1f;

    [SerializeField] private int _maxTargets = 20;
    
    public Vector3 HitCube { get => _hitCube; set => _hitCube = value; }
    public float HitOffset { get => _hitOffset; set => _hitOffset = value; }
    public int MaxTargets { get => _maxTargets; set => _maxTargets = value; }

    private Collider[] _hitColliders;

    private IHittable _owner;

    private void Awake()
    {
        _hitColliders = new Collider[_maxTargets];
        _owner = GetComponentInParent<IHittable>();
    }

    protected BossElement _element = BossElement.Fire;

    public virtual void SetElement(BossElement element)
    {
        _element = element;
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            switch (element)
            {
                case BossElement.Fire: renderer.material.color = Color.red; break;
                case BossElement.Ice: renderer.material.color = Color.cyan; break;
                case BossElement.Earth: renderer.material.color = new Color(0.4f, 0.2f, 0f); break; // Brown
                case BossElement.Aether: renderer.material.color = Color.magenta; break;
            }
        }
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
                if (_owner is Boss)
                {
                    ApplyElementalStatus(target);
                }
            }
        }
    }

    private void ApplyElementalStatus(IHittable target)
    {
        if (target is IStatusEffectReceiver player)
        {
            switch (_element)
            {
                case BossElement.Ice:
                    player.ApplySlow(0.5f, 3f);
                    break;
                case BossElement.Fire:
                    player.ApplyBurn(2, 3f);
                    break;
                case BossElement.Earth:
                    target.GetHit(_dmg, DamageType.Melee);
                    break;
                case BossElement.Aether:
                    if (_owner is Character boss)
                    {
                        boss.SetHealth(Mathf.Min(boss.MaxHP, boss.HP + 5));
                    }
                    break;
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