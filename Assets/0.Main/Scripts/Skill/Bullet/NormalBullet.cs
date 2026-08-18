using UnityEngine;

public class NormalBullet : BaseProjectile
{
    private float _damage;
    private GameObject _caster;

    public void Initialize(float damage, Vector3 direction, float speed, float lifeTime, GameObject caster = null)
    {
        InitializeMovement(direction, speed, lifeTime, false);
        _damage = damage;
        _caster = caster;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        _damage = 0f;
        _caster = null;
    }

    protected override bool ProcessHit(Collider other)
    {
        if (_caster != null && other.gameObject == _caster) return false;

        if (other.TryGetComponent(out IDamage damageableTarget))
        {
            damageableTarget.TakeDamage(_damage, _caster);
        }

        return true;
    }
}