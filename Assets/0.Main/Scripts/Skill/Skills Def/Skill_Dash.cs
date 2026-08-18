using UnityEngine;

[System.Serializable]
public class Skill_Dash : BaseSkill
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _dashDistance = 3f;
    [SerializeField] private float _dashDuration = 0.5f;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private LineRenderer _dashLineRenderer;
    [SerializeField] private GameObject _explosionParticle;
    [SerializeField] private float _cameraTrauma;

    private readonly Collider[] _hitColliders = new Collider[20];
    private IDashable _movementController;

    private bool _isDashing;
    private float _dashTimer;

    public override void Initialize(GameObject caster, Skill_InfoSO skillInfo, StatsManager statsManager)
    {
        base.Initialize(caster, skillInfo, statsManager);
        _movementController = caster.GetComponent<IDashable>();
    }

    public override void UpdateSkill(float deltaTime)
    {
        base.UpdateSkill(deltaTime);

        if (_isDashing)
        {   
            ProcessDash(deltaTime);
            if (_dashLineRenderer != null)
            {
                _dashLineRenderer.SetPosition(0, Caster.transform.position);
            }
        }
    }

    public override void Execute()
    {   
        if (IsOnInternalCooldown || _isDashing || Caster == null || _movementController == null) return;

        _isDashing = true;
        _dashTimer = 0f;
        
        float calculatedSpeed = _dashDistance / _dashDuration;
        Vector3 dashVelocity = Caster.transform.forward * calculatedSpeed;
        
        if (_dashLineRenderer != null)
        {
            _dashLineRenderer.enabled = true;
            _dashLineRenderer.SetPosition(1, Caster.transform.position);
        }
        
        _movementController.BeginDash(dashVelocity);
        StartInternalCooldown();
    }

    private void ProcessDash(float deltaTime)
    {
        _dashTimer += deltaTime;

        if (_dashTimer >= _dashDuration)
        {
            EndExecute();
        }
    }

    public override void EndExecute()
    {
        if (!_isDashing) return;

        _isDashing = false;
        _movementController.EndDash();
        TriggerExplosion();
        
        if (_dashLineRenderer != null)
        {
            _dashLineRenderer.enabled = false;
        }
    }

    private void TriggerExplosion()
    {
        float finalDamage = GetFinalDamage();
        int hitCount = Physics.OverlapSphereNonAlloc(Caster.transform.position, _explosionRadius, _hitColliders, _enemyLayer);

        for (int i = 0; i < hitCount; i++)
        {
            if (_hitColliders[i].TryGetComponent(out IDamage target))
            {   
                target.TakeDamage(finalDamage, Caster);
            }
        }
        EventBus<CameraShakeEvent>.Raise(new CameraShakeEvent { TraumaAmount = _cameraTrauma });
        SpawnExplosionParticle();
    }

    private void SpawnExplosionParticle()
    {
        if (_explosionParticle == null || ObjectPoolingManager.Instance == null) return;

        GameObject particleObj = ObjectPoolingManager.Instance.Get(_explosionParticle);
        particleObj.transform.SetPositionAndRotation(Caster.transform.position, Quaternion.identity);
    }
}