using System;
using UnityEngine;

[Serializable]
public class BoomSkill : BaseSkill
{
    [SerializeField] private GameObject boomPrefab;
    [SerializeField] private float launchForce;

    public override void Execute()
    {
        if (IsOnInternalCooldown || Caster == null || boomPrefab == null) return;

        GameObject boomObj = ObjectPoolingManager.Instance.Get(boomPrefab);
        boomObj.transform.position = Caster.transform.position;
        
        if (boomObj.TryGetComponent(out Boom boomComponent))
        {   float dmg = BaseDamage * (1+ Character_Stats.CurrentDamageMultiplier);
            boomComponent.Initialize(dmg, Range, launchForce);
        }

        StartInternalCooldown();
    }

    public override void EndExecute()
    {
    }
}