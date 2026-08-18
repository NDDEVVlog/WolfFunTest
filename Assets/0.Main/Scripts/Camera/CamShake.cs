using Unity.Cinemachine;
using UnityEngine;

public class CamShake : MonoBehaviour, ICameraShake
{   
    [SerializeField] private CinemachineBasicMultiChannelPerlin _cameraNoise;
    [SerializeField] private bool _isActive = true;
    [Range(0, 1)] [SerializeField] private float _trauma;
    [SerializeField] private float _traumaDecay = 1.3f;
    [SerializeField] private float _amplitudeMultiplier = 10f;

    private void OnEnable()
    {
        EventBus<CameraShakeEvent>.OnEvent += HandleShakeEvent;
    }

    private void OnDisable()
    {
        EventBus<CameraShakeEvent>.OnEvent -= HandleShakeEvent;
        
        if (_cameraNoise != null)
        {
            _cameraNoise.AmplitudeGain = 0f;
        }
    }

    private void HandleShakeEvent(CameraShakeEvent data)
    {
        if (!_isActive) return;
        AddTrauma(data.TraumaAmount);
    }

    public void AddTrauma(float amount)
    {
        _trauma = Mathf.Clamp01(_trauma + amount);
    }

    private void Update()
    {
        if (!_isActive || _cameraNoise == null) return;

        if (_trauma > 0)
        {
            _trauma -= Time.deltaTime * _traumaDecay * (_trauma + 0.3f);
            _trauma = Mathf.Max(0, _trauma);
            
            _cameraNoise.AmplitudeGain = _trauma * _trauma * _amplitudeMultiplier;
        }
        else if (_cameraNoise.AmplitudeGain > 0)
        {
            _cameraNoise.AmplitudeGain = 0f;
        }
    }
}