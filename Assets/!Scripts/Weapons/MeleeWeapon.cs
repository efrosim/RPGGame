using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public int _dmg = 10;
    [SerializeField] private Vector3 _hitCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float _hitOffset = 1f;

    [SerializeField] private int _maxTargets = 10;
    private Collider[] _hitColliders;

    // Ссылка на владельца оружия (Игрока или Врага), чтобы не бить самого себя
    private IHittable _owner;

    private void Awake()
    {
        _hitColliders = new Collider[_maxTargets];

        // Ищем компонент IHittable на самом объекте или на его родителях.
        // Так как MeleeWeapon лежит внутри Игрока/Врага, он найдет их скрипт (PlayerController или Enemy).
        _owner = GetComponentInParent<IHittable>();
    }

    public void Use() => DealDamage();

    public void DealDamage()
    {
        Vector3 hitCenter = transform.position + transform.forward * _hitOffset + Vector3.up;
        int count = Physics.OverlapBoxNonAlloc(hitCenter, _hitCube / 2, _hitColliders, transform.rotation);

        for (int i = 0; i < count; i++)
        {
            // Пытаемся получить интерфейс IHittable у того, в кого попали
            if (_hitColliders[i].TryGetComponent(out IHittable target))
            {
                // Если тот, в кого мы попали — это владелец оружия, пропускаем его!
                if (target == _owner) continue;

                // Иначе наносим урон
                target.GetHit(_dmg, DamageType.Melee);
            }
        }
    }
}