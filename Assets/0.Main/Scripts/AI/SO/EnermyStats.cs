using UnityEngine;

[CreateAssetMenu(fileName = "EnermyStats", menuName = "ScriptableObjects/EnermyStats", order = 1)]
public class EnermyStats : CharacterStats
{
    public float BasicAttackRange;
    public float BasicAttackInnerRadius;
    public float TotalMeleeRange => BasicAttackInnerRadius + BasicAttackRange;

    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float ProjectileMaxDistance;

}
