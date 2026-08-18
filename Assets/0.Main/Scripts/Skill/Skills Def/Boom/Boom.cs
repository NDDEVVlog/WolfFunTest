using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(Collider))]
public class Boom : MonoBehaviour, IPoolObject
{
    [SerializeField] private float boomDelay = 2f;
    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Image outlineImageUI;
    [SerializeField] private RectTransform outlineRect;
    [SerializeField] private GameObject explosionParticlePrefab;

    [SerializeField] 
    [ReadOnly]
    private float _boomDamage;
    
    [SerializeField] 
    [ReadOnly]
    private float _boomRadius;
    
    private bool _isLanded;
    private bool _isTriggered;

    private Renderer _meshRenderer;
    private Rigidbody _rigidbody;
    private MaterialPropertyBlock _materialPropertyBlock;
    private IObjectPool _pool;
    private CancellationTokenSource _cancellationTokenSource;
    
    private static readonly int ProgressPropertyId = Shader.PropertyToID("_Progress");
    private readonly Collider[] _hitColliders = new Collider[20];

    private void Awake()
    {
        _meshRenderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    public void SetPool(IObjectPool pool)
    {
        _pool = pool;
    }

    public void OnSpawn()
    {
        _isLanded = false;
        _isTriggered = false;
        _rigidbody.isKinematic = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _meshRenderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        
        if (outlineImageUI != null) outlineImageUI.enabled = false;
        UpdateMaterialProgress(0);
        
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void OnDespawn()
    {
        CancelExplosionSequence();
    }

    public void Initialize(float damage, float radius, float launchForce)
    {
        _boomDamage = damage;
        _boomRadius = radius;
        
        if (outlineRect != null) 
        {
            outlineRect.sizeDelta = new Vector2(radius * 2f, radius * 2f);
        }
        
        _rigidbody.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isLanded || _isTriggered) return;

        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            ProcessLanding();
        }
    }

    private void ProcessLanding()
    {
        _isLanded = true;
        _rigidbody.isKinematic = true;

        if (outlineImageUI != null) outlineImageUI.enabled = true;

        TriggerExplosion();
    }

    private void TriggerExplosion()
    {
        if (_isTriggered) return;
        _isTriggered = true;
        
        ProcessExplosionSequenceAsync(_cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid ProcessExplosionSequenceAsync(CancellationToken cancellationToken)
    {
        float elapsedTime = 0f;

        while (elapsedTime < boomDelay)
        {
            elapsedTime += Time.deltaTime;
            UpdateMaterialProgress(elapsedTime / boomDelay);
            
            bool isCanceled = await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken).SuppressCancellationThrow();
            if (isCanceled) return;
        }

        ExecuteExplosion();
    }

    private void UpdateMaterialProgress(float progress)
    {
        _meshRenderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetFloat(ProgressPropertyId, progress);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void ExecuteExplosion()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _boomRadius, _hitColliders, damageableLayer);
        
        for (int i = 0; i < hitCount; i++)
        {   
            if (_hitColliders[i].TryGetComponent(out IDamage damageableTarget))
            {   
                damageableTarget.TakeDamage(_boomDamage, gameObject);
            }
        }

        SpawnExplosionParticle();
        ReturnToPool();
    }

    private void SpawnExplosionParticle()
    {
        if (explosionParticlePrefab == null || _pool == null) return;

        GameObject particleObj = _pool.Get(explosionParticlePrefab);
        particleObj.transform.position = transform.position;
        particleObj.transform.rotation = Quaternion.identity;
    }

    private void ReturnToPool()
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

    private void CancelExplosionSequence()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _boomRadius);
    }
}