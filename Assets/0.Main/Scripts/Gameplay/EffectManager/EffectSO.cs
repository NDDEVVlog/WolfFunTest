using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    [SerializeField] private float _duration;
    [SerializeField] private float _tickInterval;

    public float Duration => _duration;
    public float TickInterval => _tickInterval;

    public virtual void OnApply(GameObject target) { }
    public virtual void OnTick(GameObject target) { }
    public virtual void OnRemove(GameObject target) { }
}