using UnityEngine;

public interface ITargetable
{
    Vector3 TargetPosition { get; }
    bool IsValidTarget { get; }
}