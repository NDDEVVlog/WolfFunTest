using UnityEngine;

public class ChaseState : IState
{
    private readonly AI_ScriptManager _aiManager;
    private readonly IAIMachine _machine;

    public ChaseState(AI_ScriptManager aiManager, IAIMachine machine)
    {
        _aiManager = aiManager;
        _machine = machine;
    }

    public void Enter()
    {
        _aiManager.NavAgent.isStopped = false;
        _aiManager.BotAnim?.UpdateRunInput(true);
    }

    public void UpdateState()
    {
        if (_aiManager.PlayerTransform == null) return;

        _aiManager.NavAgent.SetDestination(_aiManager.PlayerTransform.position);

        float distanceToPlayer = Vector3.Distance(_aiManager.transform.position, _aiManager.PlayerTransform.position);
        
        if (distanceToPlayer <= _aiManager.Stats.BasicAttackRange)
        {
            _machine.SwitchToAttack();
        }
    }

    public void Exit()
    {
        _aiManager.NavAgent.isStopped = true;
        _aiManager.BotAnim?.UpdateRunInput(false);
    }
}