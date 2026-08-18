using UnityEngine;

[System.Serializable]
public class Skill_Hit_Normal : BaseSkill
{
    [SerializeField] private SkillChargesUI skillChargesUI;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _projectileSpeed = 20f;
    [SerializeField] private float _projectileLifeTime = 3f;
    [SerializeField] private int _currentCharges = 3;

    private readonly float[] _spreadAngles = { -15f, 0f, 15f };
    private readonly int _maxCharges = 3;
    private readonly float _replenishTime = 3.0f;
    private readonly float _fireCoolDown = 0.5f;
    
    private float _replenishTimer;
    private float _fireCooldownTimer;

    public override void Initialize(GameObject caster, Skill_InfoSO skillInfo, StatsManager statsManager)
    {
        base.Initialize(caster, skillInfo, statsManager);
        
        if (skillChargesUI != null)
        {
            skillChargesUI.Initialize(this.SkillInfo);
        }
    }

    public override void UpdateSkill(float deltaTime)
    {
        base.UpdateSkill(deltaTime);
        UpdateCooldown(deltaTime);
        UpdateCharges(deltaTime);
    }

    private void UpdateCharges(float deltaTime)
    {
        if (_currentCharges >= _maxCharges) return;

        _replenishTimer += deltaTime;
        if (_replenishTimer >= _replenishTime)
        {
            _currentCharges++;
            _replenishTimer = 0f;
            PublishChargeUpdate();
        }
    }

    private void UpdateCooldown(float deltaTime)
    {
        if (_fireCooldownTimer > 0f)
        {
            _fireCooldownTimer -= deltaTime;
        }
    }

    public override void Execute()
    {
        if (_currentCharges <= 0 || _fireCooldownTimer > 0f) return;

        FireSpreadProjectiles();
        ConsumeCharge();
        PublishChargeUpdate();
        
        _fireCooldownTimer = _fireCoolDown;
        StartInternalCooldown();
    }

    private void FireSpreadProjectiles()
    {
        float finalDamage = BaseDamage * (1+Character_Stats.CurrentDamageMultiplier);
        
        foreach (float angle in _spreadAngles)
        {
            Quaternion spreadRotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 fireDirection = (spreadRotation * _firePoint.forward).normalized;
            Quaternion projectileRotation = Quaternion.LookRotation(fireDirection);

            GameObject bulletObj = ObjectPoolingManager.Instance.Get(_projectilePrefab);
            bulletObj.transform.SetPositionAndRotation(_firePoint.position, projectileRotation);

            if (bulletObj.TryGetComponent(out NormalBullet bullet))
            {   
                Debug.Log("GG");
                bullet.Initialize(finalDamage, fireDirection, _projectileSpeed, _projectileLifeTime, Caster);
            }
        }
    }

    private void ConsumeCharge()
    {
        _currentCharges--;
    }   

    private void PublishChargeUpdate()
    {
        EventBus<SkillChargeChangedEvent>.Raise(new SkillChargeChangedEvent 
        { 
            SkillInfo = this.SkillInfo, 
            CurrentCharges = _currentCharges 
        });
    }

    public override void EndExecute()
    {
    }
}