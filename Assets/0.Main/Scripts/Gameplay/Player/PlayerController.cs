using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<PlayerState> OnStateChanged;

    public bool BlockInput { get; set; }
    public PlayerState CurrentState { get; private set; }

    private Vector2 _moveInput;

    public void SetMoveInput(Vector2 input)
    {
        if (CurrentState == PlayerState.Dead) return;
        _moveInput = input;
    }

    public Vector2 GetMoveInput() => _moveInput;

    public void Update()
    {
        if (BlockInput || CurrentState == PlayerState.Dead) return;
        UpdateState();
    }

    public void SetDeadState()
    {
        CurrentState = PlayerState.Dead;
        _moveInput = Vector2.zero;
        OnStateChanged?.Invoke(CurrentState);
    }

    private void UpdateState()
    {
        PlayerState newState = _moveInput != Vector2.zero ? PlayerState.Running : PlayerState.Idle;
        
        if (CurrentState == newState) return;
        
        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }
}