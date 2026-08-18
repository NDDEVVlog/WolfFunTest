using System;
using UnityEngine;

public class StatsManager : MonoBehaviour 
{   
    [SerializeField] private CharacterStats _baseStats;

    public float CurrentMaxHealth { get; private set; }
    public float CurrentArmor { get; private set; }
    public float CurrentDamageMultiplier { get; private set; }
    public float CurrentMoveSpeed { get; private set; }
    public float CurrentTurnSpeed { get; private set; }
    
    public int CurrentLevel { get; private set; } = 1;
    public float CurrentExp { get; private set; }
    public CharacterStats CurrentStats => _baseStats ;

    public event Action<int> OnLevelUp;
    public event Action<float, float> OnHealthIncreased;
    public event Action<float, float> OnExpChanged;


    public void InitializeStats()
    {   
        _baseStats = Instantiate(_baseStats);
        CurrentMaxHealth = _baseStats.BaseMaxHealth;
        CurrentArmor = _baseStats.BaseArmor;
        CurrentDamageMultiplier = _baseStats.BaseDamageMultiplier;
        CurrentMoveSpeed = _baseStats.MoveSpeed;
        CurrentTurnSpeed = _baseStats.TurnSpeed;
    }

    public void AddExperience(float amount)
    {
        CurrentExp += amount;
        CheckLevelUp();
        OnExpChanged?.Invoke(CurrentExp, _baseStats.ExpPerLevel);
    }

    private void CheckLevelUp()
    {
        while (CurrentExp >= _baseStats.ExpPerLevel)
        {
            CurrentExp -= _baseStats.ExpPerLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        CurrentLevel++;
        CurrentMaxHealth += _baseStats.HealthBonusPerLevel;
        CurrentArmor += _baseStats.ArmorBonusPerLevel;
        CurrentDamageMultiplier += _baseStats.DamageMultiplierBonusPerLevel;
        

        OnHealthIncreased?.Invoke(_baseStats.HealthBonusPerLevel, CurrentMaxHealth);
        OnLevelUp?.Invoke(CurrentLevel);
    }

    public float CalculateDamageDealt(float rawDamage)
    {
        return rawDamage * (1f + CurrentDamageMultiplier);
    }
}
