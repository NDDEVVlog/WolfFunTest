using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

public class WaveAnnounceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _displayDuration = 2f;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void Awake()
    {
        _canvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        EventBus<WaveStartedEvent>.OnEvent += ShowWaveAnnouncement;
    }

    private void OnDisable()
    {
        EventBus<WaveStartedEvent>.OnEvent -= ShowWaveAnnouncement;
    }

    private void ShowWaveAnnouncement(WaveStartedEvent data)
    {
        _waveText.text = $"WAVE {data.WaveNumber}";
        AnimateAnnouncement().Forget();
    }

    private async UniTaskVoid AnimateAnnouncement()
    {
        float timer = 0;
        
        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, timer / _fadeDuration);
            await UniTask.Yield();
        }
        
        _canvasGroup.alpha = 1;
        await UniTask.Delay(TimeSpan.FromSeconds(_displayDuration));

        timer = 0;
        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1, 0, timer / _fadeDuration);
            await UniTask.Yield();
        }
        
        _canvasGroup.alpha = 0;
    }
}