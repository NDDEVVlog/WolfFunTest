using System;
using UnityEngine;

public abstract class BaseSkill : ISkill
{
    protected float BaseDamage { get; private set; }
    protected float InternalCooldown { get; private set; }
    protected float Range { get; private set; }
    protected GameObject Caster { get; private set; }
    protected Skill_InfoSO SkillInfo { get; private set; }
    protected StatsManager Character_Stats{ get; private set; }
    private float _currentInternalCooldown;

    public bool IsOnInternalCooldown => _currentInternalCooldown > 0f;

    public event Action<float> OnCooldownProgressChanged;

    public virtual void Initialize(GameObject caster, Skill_InfoSO skillInfo,StatsManager characterStats  )
    {
        Caster = caster;
        BaseDamage = skillInfo.BaseDamage;
        InternalCooldown = skillInfo.Cooldown;
        Character_Stats = characterStats;
        Range = skillInfo.Range;
        this.SkillInfo = skillInfo;
        _currentInternalCooldown = 0f;
    }

    public virtual void UpdateSkill(float deltaTime)
    {
        if (!IsOnInternalCooldown) return;

        _currentInternalCooldown = Mathf.Max(0f, _currentInternalCooldown - deltaTime);
        OnCooldownProgressChanged?.Invoke(_currentInternalCooldown / InternalCooldown);
    }

    protected void StartInternalCooldown()
    {
        _currentInternalCooldown = InternalCooldown;
        OnCooldownProgressChanged?.Invoke(1f);
    }

    public float GetFinalDamage() => BaseDamage * (1+ Character_Stats.CurrentDamageMultiplier);
    public abstract void Execute();
    public abstract void EndExecute();
}