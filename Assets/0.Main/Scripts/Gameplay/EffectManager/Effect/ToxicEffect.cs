using UnityEngine;

[CreateAssetMenu(fileName = "ToxicEffect", menuName = "Effects/ToxicEffect")]
public class ToxicEffectSO : EffectSO
{
    [SerializeField] private float _damagePerTick = 30f;

    public override void OnApply(GameObject target)
    {
        if (target.TryGetComponent(out IDamage damageable))
        {
            damageable.TakeDamage(_damagePerTick,null);
        }
    }

    public override void OnTick(GameObject target)
    {
        if (target.TryGetComponent(out IDamage damageable))
        {
            damageable.TakeDamage(_damagePerTick,null);
        }
    }
}