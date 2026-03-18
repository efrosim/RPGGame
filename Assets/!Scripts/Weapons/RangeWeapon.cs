using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public int _dmg = 10;
    public GameObject _shellPrefab;
    public Transform _shellSpawnPos;

    public void Shoot()
    {
        GameObject shell = Instantiate(_shellPrefab, _shellSpawnPos.position, _shellSpawnPos.rotation);
        shell.GetComponent<Shell>().SetDamage(_dmg);
    }
}