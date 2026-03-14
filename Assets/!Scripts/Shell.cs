using UnityEngine;

public class Shell : MonoBehaviour
{
    public float _speed;
    private int _damage;

    public void SetDamage(int dmg)
    {
        _damage = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IHittable target))
        {
            target.GetHit(_damage, DamageType.Range); // Магический урон
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * transform.forward;
    }
}
