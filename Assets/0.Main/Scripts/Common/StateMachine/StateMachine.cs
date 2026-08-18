using System;

[Serializable]
public abstract class StateMachine
{
    protected IState CurrentState;
    protected AI_ScriptManager AIManager;

    public virtual void Initialize(AI_ScriptManager aiManager)
    {
        AIManager = aiManager;
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void Update()
    {
        CurrentState?.UpdateState();
    }

    public void Stop()
    {
        CurrentState?.Exit();
        CurrentState = null;
    }
}