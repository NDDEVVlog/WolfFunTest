using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class CharacterStats : ScriptableObject
{
    [Header("Base Stats")]
    public float BaseMaxHealth = 500f;
    public float MoveSpeed = 2f;
    public float MaxSpeed = 3f;
    public float TurnSpeed = 180f;
    public float BaseArmor = 0f;
    public float BaseDamageMultiplier = 0f;

    [Header("Level Up Scaling")]
    public float ExpPerLevel = 100f;
    public float HealthBonusPerLevel = 40f;
    public float ArmorBonusPerLevel = 2f;
    public float DamageMultiplierBonusPerLevel = 0.1f;

    [Header("Basic Attack")]
    public float BasicAttackDamage = 10f;
    public int BasicAttackMaxCharges = 3;
    public float BasicAttackChargeTime = 3f;
    public float BasicAttackCooldown = 0.5f;
    public float BasicAttackSpreadAngle = 15f;
    public int BasicAttackProjectileCount = 3;
}
