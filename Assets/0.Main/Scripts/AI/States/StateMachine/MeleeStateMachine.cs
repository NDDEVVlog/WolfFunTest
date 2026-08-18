using System;

[Serializable]
public class MeleeStateMachine : StateMachine, IAIMachine
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
        ChangeState(new MeleeAttackState(AIManager, this));
    }

    public void SwitchToCooldown()
    {
        ChangeState(new CooldownState(AIManager, this));
    }
}