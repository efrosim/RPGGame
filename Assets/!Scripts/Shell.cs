using UnityEngine;

public class Shell : MonoBehaviour
{
    public float _speed;

    public EnemyRange _ownedBy;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.TryGetComponent(out IHittable target))
        {
            target.GetHit(_ownedBy.GetDmg());
            Debug.Log("DmdDealed");
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += _speed * Time.fixedDeltaTime * transform.forward;
    }
}
