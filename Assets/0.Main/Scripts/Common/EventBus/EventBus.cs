using System;

public static class EventBus<T> where T : struct
{
    public static event Action<T> OnEvent;

    public static void Raise(T eventArgs)
    {
        OnEvent?.Invoke(eventArgs);
    }
}

public struct SkillChargeChangedEvent
{
    public Skill_InfoSO SkillInfo;
    public int CurrentCharges;
}

public struct EnemyDeathEvent
{
    public float ExpGranted;
}

public struct WaveStartedEvent
{
    public int WaveNumber;
}

public struct EnemySpawnedEvent { }

public struct CameraShakeEvent
{
    public float TraumaAmount;
}