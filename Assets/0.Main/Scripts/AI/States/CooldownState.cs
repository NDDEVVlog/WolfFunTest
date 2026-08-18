using UnityEngine;

public class CooldownState : IState
{
    private readonly AI_ScriptManager _aiManager;
    private float _timer;

    public CooldownState(AI_ScriptManager aiManager)
    {
        _aiManager = aiManager;
    }

    public void Enter()
    {
        _timer = _aiManager.Stats.BasicAttackCooldown;
    }

    public void UpdateState()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _aiManager.ChangeState(new ChaseState(_aiManager));
        }
    }

    public void Exit() { }
}