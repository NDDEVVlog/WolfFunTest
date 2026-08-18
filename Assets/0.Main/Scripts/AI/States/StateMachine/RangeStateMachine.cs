using System;

[Serializable]
public class RangeStateMachine : StateMachine, IAIMachine
{
    public override void Initialize(AI_ScriptManager aiManager)
    {
        base.Initialize(aiManager);
        SwitchToChase();
    }

    public void SwitchToChase()
    {
        ChangeState(new ChaseState(AIManager, this));
    }

    public void SwitchToAttack()
    {
        ChangeState(new RangedAttackState(AIManager, this));
    }

    public void SwitchToCooldown()
    {
        ChangeState(new CooldownState(AIManager, this));
    }
}