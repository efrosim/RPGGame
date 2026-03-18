using UnityEngine;

// DIP: Враги зависят от абстракции цели, а не от Transform игрока
public interface ITargetable
{
    Vector3 TargetPosition { get; }
    bool IsValidTarget { get; }
}