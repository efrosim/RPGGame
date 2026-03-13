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
        Debug.Log("Hit");
        if (collision.gameObject.TryGetComponent(out IHittable target))
        {
            target.GetHit(_damage);
            Debug.Log("DmdDealed");
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * transform.forward;
    }
}
