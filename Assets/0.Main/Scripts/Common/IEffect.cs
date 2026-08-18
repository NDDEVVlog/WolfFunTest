using UnityEngine;

public interface IEffectHandler
{
    void ApplyEffect(EffectSO effect);
    void RemoveEffect(EffectSO effect);
}