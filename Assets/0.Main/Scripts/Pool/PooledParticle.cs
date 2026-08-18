using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticle : MonoBehaviour, IPoolObject
{
    private ParticleSystem _particleSystem;
    private IObjectPool _pool;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void SetPool(IObjectPool pool)
    {
        _pool = pool;
    }

    public void OnSpawn()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _particleSystem.Play();
        WaitAndReturnToPoolAsync(_cancellationTokenSource.Token).Forget();
    }

    public void OnDespawn()
    {
        CancelTask();
        _particleSystem.Stop();
        _particleSystem.Clear();
    }

    private async UniTaskVoid WaitAndReturnToPoolAsync(CancellationToken cancellationToken)
    {
        float duration = _particleSystem.main.duration;
        
        bool isCanceled = await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: cancellationToken).SuppressCancellationThrow();
        
        if (isCanceled) return;

        if (_pool != null)
        {
            _pool.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CancelTask()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}