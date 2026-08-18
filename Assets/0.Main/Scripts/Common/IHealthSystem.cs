using System;

public interface IHealthSystem
{
    event Action<float, float> OnHealthChanged;
}