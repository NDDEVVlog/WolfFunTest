using UnityEngine;

public class PlayerMovementManager : MonoBehaviour, IDashable
{
    private Rigidbody _rigidbody;
    private CharacterStats _stats;
    private Transform _cameraTransform;

    private PlayerState _currentState;
    private Vector2 _moveInput;
    private Vector3 _currentMoveDirection;
    
    private bool _isExternalControlled;
    private Vector3 _externalVelocity;

    public void Initialize(Rigidbody rb, CharacterStats stats)
    {
        _rigidbody = rb;
        _stats = stats;
        
        if (Camera.main != null) 
            _cameraTransform = Camera.main.transform;
    }

    public void UpdateCurrentState(PlayerState newState) => _currentState = newState;

    public void SetMoveData(Vector2 input) => _moveInput = input;

    public void BeginDash(Vector3 velocity)
    {
        _isExternalControlled = true;
        _externalVelocity = velocity;
    }

    public void EndDash()
    {
        _isExternalControlled = false;
        _externalVelocity = Vector3.zero;
    }

    public void FixedUpdate()
    {
        if (_isExternalControlled)
        {
            ApplyExternalMovement();
            return;
        }

        CalculateMoveDirection();
        ApplyMovement();
        RotatePlayer();
    }

    private void ApplyExternalMovement()
    {
        _rigidbody.linearVelocity = new Vector3(_externalVelocity.x, _rigidbody.linearVelocity.y, _externalVelocity.z);
    }

    private void CalculateMoveDirection()
    {
        if (_cameraTransform == null || _moveInput == Vector2.zero)
        {
            _currentMoveDirection = Vector3.zero;
            return;
        }

        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        _currentMoveDirection = (camRight.normalized * _moveInput.x + camForward.normalized * _moveInput.y).normalized;
    }

    private void ApplyMovement()
    {
        if (_currentMoveDirection == Vector3.zero || _currentState != PlayerState.Running)
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            return;
        }

        Vector3 targetVelocity = _currentMoveDirection * _stats.MoveSpeed;
        _rigidbody.linearVelocity = new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.z);
    }

    private void RotatePlayer()
    {
        if (_currentMoveDirection == Vector3.zero)
        {
            _rigidbody.angularVelocity = Vector3.zero;
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(_currentMoveDirection);
        Quaternion nextRotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _stats.TurnSpeed * Time.fixedDeltaTime);
        
        _rigidbody.MoveRotation(nextRotation);
    }
}