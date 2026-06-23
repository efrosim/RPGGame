using UnityEngine;

public class RangeWeapon : MonoBehaviour, IWeapon
{
    public int _dmg = 10;
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;

    protected BossElement _element = BossElement.Fire;
    private IHittable _owner;

    private void Awake()
    {
        _owner = GetComponentInParent<IHittable>();
    }

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

    public virtual void Shoot()
    {
        GameObject shellObj = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        if (shellObj.TryGetComponent(out Shell shell))
        {
            shell.Initialize(_dmg, _element, _owner);
        }
    }
    
    public virtual void Use() => Shoot();
}