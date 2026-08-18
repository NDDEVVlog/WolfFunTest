using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour, IEffectHandler
{
    private class ActiveEffect
    {
        public EffectSO EffectData;
        public float DurationTimer;
        public float TickTimer;
    }

    private readonly List<ActiveEffect> _activeEffects = new List<ActiveEffect>();

    private void Update()
    {
        ProcessEffects();
    }

    public void ApplyEffect(EffectSO effect)
    {   
        Debug.Log("Apply Effect");
        ActiveEffect existingEffect = _activeEffects.Find(e => e.EffectData == effect);

        if (existingEffect != null)
        {
            existingEffect.DurationTimer = effect.Duration;
            return;
        }

        effect.OnApply(gameObject);

        _activeEffects.Add(new ActiveEffect
        {
            EffectData = effect,
            DurationTimer = effect.Duration,
            TickTimer = effect.TickInterval
        });
    }

    public void RemoveEffect(EffectSO effect)
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            if (_activeEffects[i].EffectData == effect)
            {
                effect.OnRemove(gameObject);
                _activeEffects.RemoveAt(i);
            }
        }
    }

    private void ProcessEffects()
    {
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffect active = _activeEffects[i];
            
            active.DurationTimer -= Time.deltaTime;

            if (active.EffectData.TickInterval > 0f)
            {
                active.TickTimer -= Time.deltaTime;
                if (active.TickTimer <= 0f)
                {
                    active.EffectData.OnTick(gameObject);
                    active.TickTimer = active.EffectData.TickInterval;
                }
            }

            if (active.DurationTimer <= 0f)
            {
                active.EffectData.OnRemove(gameObject);
                _activeEffects.RemoveAt(i);
            }
        }
    }
}