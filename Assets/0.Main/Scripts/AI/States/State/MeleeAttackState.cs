using UnityEngine;

public class MeleeAttackState : IState
{
    private readonly AI_ScriptManager _aiManager;
    private readonly IAIMachine _machine;
    private float _attackDuration;

    public MeleeAttackState(AI_ScriptManager aiManager, IAIMachine machine)
    {
        _aiManager = aiManager;
        _machine = machine;
    }

    public void Enter()
    {
        _attackDuration = 0.5f; 
        _aiManager.BotAnim.AttackAnim(true);
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
        _aiManager.BotAnim.AttackAnim(false);
    }
}