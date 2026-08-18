using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Effects/BurnEffect")]
public class BurnEffectSO : EffectSO
{
    [SerializeField] private float _burnDamage = 15f;

    public override void OnTick(GameObject target)
    {
        if (target.TryGetComponent(out IDamage damageable))
        {
            damageable.TakeDamage(_burnDamage,null);
        }
    }
}