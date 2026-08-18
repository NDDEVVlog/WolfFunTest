using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    [SerializeField] private AI_ScriptManager _aiManager;


    public void ExecuteHit()
    {
        if (_aiManager.PlayerTransform == null) return;

        Vector3 directionToPlayer = (_aiManager.PlayerTransform.position - _aiManager.transform.position).normalized;
        float angleToPlayer = Vector3.Angle(_aiManager.transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(_aiManager.transform.position, _aiManager.PlayerTransform.position);

        if (distanceToPlayer <= _aiManager.Stats.TotalMeleeRange && angleToPlayer <= _aiManager.Stats.BasicAttackSpreadAngle / 2f)
        {
            if (_aiManager.PlayerTransform.TryGetComponent(out IDamage damageablePlayer))
            {
                damageablePlayer.TakeDamage(_aiManager.Stats.BasicAttackDamage, _aiManager.gameObject);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position,_aiManager.Stats.TotalMeleeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,_aiManager.Stats.BasicAttackInnerRadius);
    }
}