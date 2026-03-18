using System;

public interface IHealth
{
    int HP { get; }
    int MaxHP { get; }
    event Action<float> OnHealthChanged;
    float GetHealthNormalized();
}