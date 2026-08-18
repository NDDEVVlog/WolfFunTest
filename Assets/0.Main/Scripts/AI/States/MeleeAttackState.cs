using UnityEngine;

public class MeleeAttackState : IState
{
    private readonly AI_ScriptManager _aiManager;
    private float _attackDuration;

    public MeleeAttackState(AI_ScriptManager aiManager)
    {
        _aiManager = aiManager;
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
            _aiManager.ChangeState(new CooldownState(_aiManager));
        }
    }

    public void Exit()
    {
        _aiManager.BotAnim.AttackAnim(false);
    }
}