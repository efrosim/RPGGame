using System.Xml.Serialization;
using UnityEditor.Animations;
using UnityEngine;

public enum DamageType
{
    Melee,
    Range
}

public abstract class Character : MonoBehaviour, IHittable
{
    [Header("Stats")]
    [SerializeField] protected int _HP;
    [SerializeField] protected int _MaxHP;
    [SerializeField] protected int _dmg;
    public float _reload;
    public float _moveSpeed;
    public float _rotSpeed;

    public Animator _animator;
    //Audio Controller?

    private void Start()
    {
        _HP = _MaxHP;
    }

    public virtual void GetHit(int dmg)
    {
        _HP -= dmg;
        //Anim of get damage
 //       if (_HP < 1) Dead();
    }

    public virtual void DealDmg()
    {
        
    }
}
