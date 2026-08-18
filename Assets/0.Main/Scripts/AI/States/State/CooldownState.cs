using UnityEngine;

public class CooldownState : IState
{
    private readonly AI_ScriptManager _aiManager;
    private readonly IAIMachine _machine;
    private float _timer;

    public CooldownState(AI_ScriptManager aiManager, IAIMachine machine)
    {
        _aiManager = aiManager;
        _machine = machine;
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
            _machine.SwitchToChase();
        }
    }

    public void Exit() { }
}