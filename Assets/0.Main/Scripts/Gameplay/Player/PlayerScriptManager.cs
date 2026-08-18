using System;
using UnityEngine;

[Serializable]
public enum PlayerState
{
    Idle, Running, Dead
}

[RequireComponent(typeof(InputReader), typeof(PlayerController), typeof(PlayerMovementManager))]
[RequireComponent(typeof(PlayerAnimManager))]
public class PlayerScriptManager : MonoBehaviour
{
    public InputReader InputReader;
    public PlayerController Controller;
    public PlayerMovementManager MovementManager;
    public PlayerAnimManager AnimManager;
    public StatsManager StatsManager;
    public SkillHolderManager SkillHolderManager;
    public HealthController HealthController;
    public Rigidbody PlayerRB;

    private void Awake()
    {
        InitializeDependencies();
    }

    private void OnEnable()
    {
        BindEvents();
    }

    private void OnDisable()
    {
        UnbindEvents();
    }

    private void InitializeDependencies()
    {   
        PlayerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        StatsManager.InitializeStats();
        MovementManager.Initialize(PlayerRB, StatsManager.CurrentStats);
        SkillHolderManager.Init(StatsManager);
    }

    private void BindEvents()
    {
        InputReader.OnMoveEvent += HandleInputMove;
        InputReader.OnAttackEvent += HandleAttack;
        InputReader.OnSkillOnePressEvent += HandleSkillOne;
        InputReader.OnSkillTwoPressEvent += HandleSkillTwo;

        Controller.OnStateChanged += MovementManager.UpdateCurrentState;
        Controller.OnStateChanged += AnimManager.HandleStateChanged;

        HealthController.OnDeath += HandlePlayerDeath;
        EventBus<EnemyDeathEvent>.OnEvent += HandleEnemyKilled;
    }

    private void UnbindEvents()
    {
        InputReader.OnMoveEvent -= HandleInputMove;
        InputReader.OnAttackEvent -= HandleAttack;
        InputReader.OnSkillOnePressEvent -= HandleSkillOne;
        InputReader.OnSkillTwoPressEvent -= HandleSkillTwo;

        Controller.OnStateChanged -= MovementManager.UpdateCurrentState;
        Controller.OnStateChanged -= AnimManager.HandleStateChanged;

        HealthController.OnDeath -= HandlePlayerDeath;
        EventBus<EnemyDeathEvent>.OnEvent -= HandleEnemyKilled;
    }

    private void HandleInputMove(Vector2 input)
    {
        if (Controller.CurrentState == PlayerState.Dead) return;
        Controller.SetMoveInput(input);
        MovementManager.SetMoveData(input);
    }

    private void HandleAttack()
    {
        if (Controller.CurrentState == PlayerState.Dead) return;
        SkillHolderManager.skillSlots[0].Skill.Execute();
    }

    private void HandleSkillOne()
    {
        if (Controller.CurrentState == PlayerState.Dead) return;
        SkillHolderManager.skillSlots[1].Skill.Execute();
    }

    private void HandleSkillTwo()
    {
        if (Controller.CurrentState == PlayerState.Dead) return;
        SkillHolderManager.skillSlots[2].Skill.Execute();
    }

    private void HandlePlayerDeath()
    {
        Controller.SetDeadState();
        MovementManager.SetMoveData(Vector2.zero);
        AnimManager.DeathEventCall();
    }

    private void HandleEnemyKilled(EnemyDeathEvent data)
    {
        StatsManager.AddExperience(data.ExpGranted);
    }
}