public interface IHittable
{
    // Передаем тип урона
    void GetHit(int dmg, DamageType type);
}