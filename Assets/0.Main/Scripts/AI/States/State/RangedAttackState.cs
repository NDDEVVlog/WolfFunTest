using UnityEngine;

public class RangedAttackState : IState
{
    private readonly AI_ScriptManager _aiManager;
    private readonly IAIMachine _machine;
    private float _attackDuration;

    public RangedAttackState(AI_ScriptManager aiManager, IAIMachine machine)
    {
        _aiManager = aiManager;
        _machine = machine;
    }

    public void Enter()
    {
        _attackDuration = 0.5f; 
        
        if (_aiManager.BotAnim != null)
        {
            _aiManager.BotAnim.AttackAnim(true);
        }
        
        ExecuteRangedAttack();
    }

    public void UpdateState()
    {
        _attackDuration -= Time.deltaTime;
        if (_attackDuration <= 0f)
        {
            _machine.SwitchToCooldown();
        }
    }

    public void Exit()
    {
        if (_aiManager.BotAnim != null)
        {
            _aiManager.BotAnim.AttackAnim(false);
        }
    }

    private void ExecuteRangedAttack()
    {
        if (_aiManager.PlayerTransform == null || _aiManager.Stats.ProjectilePrefab == null) return;

        Vector3 direction = (_aiManager.PlayerTransform.position - _aiManager.ProjectileSpawnPoint.position).normalized;
        direction.y = 0;

        Quaternion rotation = direction != Vector3.zero ? Quaternion.LookRotation(direction) : Quaternion.identity;

        GameObject projectileObj = ObjectPoolingManager.Instance.Get(_aiManager.Stats.ProjectilePrefab);
        projectileObj.transform.SetPositionAndRotation(_aiManager.ProjectileSpawnPoint.position, rotation);

        if (projectileObj.TryGetComponent(out ToxicProjectile projectile))
        {
            projectile.Initialize(direction, _aiManager.Stats.ProjectileSpeed, _aiManager.Stats.ProjectileMaxDistance);
        }
    }
}