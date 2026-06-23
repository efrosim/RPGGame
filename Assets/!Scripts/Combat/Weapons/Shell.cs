using UnityEngine;

public class Shell : MonoBehaviour
{
    public float _speed;
    private int _damage;
    private BossElement _element;
    private IHittable _owner;

    public void SetDamage(int dmg) => _damage = dmg;

    public void Initialize(int dmg, BossElement element, IHittable owner)
    {
        _damage = dmg;
        _element = element;
        _owner = owner;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IHittable target))
        {
            if (_owner != null && target == _owner) return;
            
            target.GetHit(_damage, DamageType.Range); 

            if (_owner is Boss)
            {
                ApplyElementalStatus(target);
            }
        }
        Destroy(gameObject);
    }

    private void ApplyElementalStatus(IHittable target)
    {
        if (target is PlayerView player)
        {
            switch (_element)
            {
                case BossElement.Ice:
                    player.ApplySlow(0.5f, 3f);
                    break;
                case BossElement.Fire:
                    player.ApplyBurn(2, 3);
                    break;
                case BossElement.Earth:
                    player.GetHit(_damage, DamageType.Range);
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

    private void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * transform.forward;
    }
}