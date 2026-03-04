using System.Xml.Serialization;
using UnityEditor.Animations;
using UnityEngine;

public enum DamageType
{
    Melee,
    Range
}

public abstract class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int _HP;
    [SerializeField] protected int _dmgMelee;
    [SerializeField] protected int _dmgRange;
    public float _reload;
    public float _moveSpeed;
    public float _rotSpeed;

    private AnimatorController _animController;
    //Audio Controller?

    virtual public void GetDmg(int dmg)
    {
        _HP -= dmg;
        //Anim of get damage
 //       if (_HP < 1) Dead();
    }

    virtual public int DealDmg(DamageType type)
    {
        switch (type)
        { 
            case DamageType.Melee:
                return _dmgMelee;
            case DamageType.Range:
                return _dmgRange;
            default:
                return 0;
        }
    }
}
