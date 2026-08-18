using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour, IPoolObject
{
    [SerializeField] private GameObject _hitParticle;
    
    protected Vector3 Direction;
    protected float Speed;
    
    private float _lifeTime;
    private float _currentLifeTime;
    private IObjectPool _pool;

    public void SetPool(IObjectPool pool) => _pool = pool;

    public virtual void OnSpawn()
    {
        _currentLifeTime = 0f;
    }

    public virtual void OnDespawn()
    {
        Direction = Vector3.zero;
        Speed = 0f;
    }

    protected void InitializeMovement(Vector3 direction, float speed, float maxDistanceOrLifeTime, bool isDistanceBased)
    {
        Direction = direction.normalized;
        Speed = speed;
        _lifeTime = isDistanceBased ? (maxDistanceOrLifeTime / speed) : maxDistanceOrLifeTime;
    }

    private void Update()
    {
        transform.position += Direction * (Speed * Time.deltaTime);

        _currentLifeTime += Time.deltaTime;
        if (_currentLifeTime >= _lifeTime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ProcessHit(other))
        {
            SpawnHitParticle();
            ReturnToPool();
        }
    }

    protected abstract bool ProcessHit(Collider other);

    private void SpawnHitParticle()
    {
        if (_hitParticle == null || ObjectPoolingManager.Instance == null) return;

        GameObject particleObj = ObjectPoolingManager.Instance.Get(_hitParticle);
        particleObj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    protected void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}