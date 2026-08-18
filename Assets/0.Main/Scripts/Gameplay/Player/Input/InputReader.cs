using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, _controlInputs.IPlayerActions
{   
    public event Action<Vector2> OnMoveEvent;
    public event Action OnAttackEvent;
    public event Action OnSkillOnePressEvent;
    public event Action OnSkillTwoPressEvent;


    private _controlInputs _inputControls;
    private Vector2 _currentInput;

    private void Awake()
    {
        _inputControls = new _controlInputs();
        _inputControls.Player.SetCallbacks(this);
    }

    private void OnEnable() => _inputControls.Enable();
    private void OnDisable() => _inputControls.Disable();

    private void Update()
    {
        // if (JoystickPA != null && (JoystickPA.GetHorizontalAxis() != 0 || JoystickPA.GetVerticalAxis() != 0))
        // {
        //     _currentInput = new Vector2(JoystickPA.GetHorizontalAxis(), JoystickPA.GetVerticalAxis());
        //     OnMoveEvent?.Invoke(_currentInput);
        // }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _currentInput = context.ReadValue<Vector2>();
        OnMoveEvent?.Invoke(_currentInput);
    }

    public void OnRoll(InputAction.CallbackContext context) {}
    public void OnAttack(InputAction.CallbackContext context)
    {   

        if (context.performed)
        {   

            OnAttackEvent?.Invoke();

        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }
    public void OnNext(InputAction.CallbackContext context) {}
    public void OnPrevious(InputAction.CallbackContext context) {}

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.performed)
        {   

            OnSkillOnePressEvent?.Invoke();

        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {   
            
            OnSkillTwoPressEvent?.Invoke();

        }
    }
}