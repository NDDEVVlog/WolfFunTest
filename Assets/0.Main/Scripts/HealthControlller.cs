using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class HealthController : MonoBehaviour, IDamage
{
    private StatsManager _statManager;
    private bool _isDead;
    
    public float CurrentHealth { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        _statManager = GetComponent<StatsManager>();
    }

    private void Start()
    {
        ResetHealth();
        _statManager.OnHealthIncreased += HandleHealthIncreased;
    }

    private void OnDestroy()
    {
        if (_statManager != null)
        {
            _statManager.OnHealthIncreased -= HandleHealthIncreased;
        }
    }

    public void ResetHealth()
    {
        _isDead = false;
        CurrentHealth = _statManager.CurrentMaxHealth;
        NotifyHealthChanged();
    }

    [Button("DealDamge")]
    public void TakeDamage(float rawDamage, GameObject caster)
    {   
        if (_isDead) return;

        float finalDamage = Mathf.Max(0, rawDamage - _statManager.CurrentArmor);
        CurrentHealth = Mathf.Max(0, CurrentHealth - finalDamage);

        NotifyHealthChanged();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void HandleHealthIncreased(float bonusHealth, float newMaxHealth)
    {
        if (_isDead) return;
        
        CurrentHealth += bonusHealth;
        NotifyHealthChanged();
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(CurrentHealth, _statManager.CurrentMaxHealth);
    }

    private void Die()
    {
        _isDead = true;
        OnDeath?.Invoke();
    }
}