using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(CapsuleCollider))]
public class AI_ScriptManager : MonoBehaviour, IPoolObject
{
    [SerializeField] private StatsManager _enemyStats;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private BotAnimController _botAnim;
    [SerializeField] private HealthController _healthController;
    [SerializeField] private float _despawnDelay = 2.5f;
    
    private NavMeshAgent _navAgent;
    private Rigidbody _rb;
    private CapsuleCollider _collider;
    private Transform _playerTransform;
    private StateMachine _stateMachine;
    private IObjectPool _pool;
    private CancellationTokenSource _cts;

    public EnermyStats Stats => (EnermyStats)_enemyStats.CurrentStats;
    public Transform PlayerTransform => _playerTransform;
    public NavMeshAgent NavAgent => _navAgent;
    public Transform ProjectileSpawnPoint => _projectileSpawnPoint;
    public BotAnimController BotAnim => _botAnim;

    private void Awake()
    {   
        _enemyStats.InitializeStats();
        _navAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _stateMachine = new StateMachine();
        
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _navAgent.speed = Stats.MoveSpeed;
        _navAgent.stoppingDistance = Stats.BasicAttackRange;
        _navAgent.enabled = false; 
    }

    private void OnEnable()
    {   
        if (_healthController == null) return;
        _healthController.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {   
        if (_healthController == null) return;
        _healthController.OnDeath -= HandleDeath;       
    }

    private void Start()
    {
        if (_pool == null)
        {
            OnSpawn();
            ActivateAI();
        }
    }

    private void Update()
    {
        if (_healthController.CurrentHealth <= 0 || !_navAgent.enabled) return;
        _stateMachine.Update();
    }

    public void SetPool(IObjectPool pool)
    {
        _pool = pool;
    }

    public void OnSpawn()
    {
        _cts = new CancellationTokenSource();
        _healthController.ResetHealth();
        
        if (_botAnim != null)
        {
            _botAnim.ResetAnimations();
        }
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    public void ActivateAI()
    {
        _collider.enabled = true;
        _navAgent.enabled = true;
        
        if (_playerTransform != null)
        {
            ChangeState(new ChaseState(this));
        }
    }

    public void OnDespawn()
    {
        CancelTask();
        
        _stateMachine.ChangeState(null);
        
        if (_navAgent != null && _navAgent.isOnNavMesh)
        {
            _navAgent.isStopped = true;
        }
        _navAgent.enabled = false;
        _collider.enabled = false;
    }

    public void ChangeState(IState newState)
    {
        if (_healthController.CurrentHealth <= 0) return;
        _stateMachine.ChangeState(newState);
    }

    private void HandleDeath()
    {   
        if (_botAnim != null)
        {
            _botAnim.Die();
        }

        _stateMachine.ChangeState(null);

        if (_navAgent != null && _navAgent.isOnNavMesh)
        {
            _navAgent.isStopped = true;
        }
        _navAgent.enabled = false;
        _collider.enabled = false;
        
        EventBus<EnemyDeathEvent>.Raise(new EnemyDeathEvent { ExpGranted = 1000f });
        ReturnToPoolAsync(_cts.Token).Forget();
    }

    private async UniTaskVoid ReturnToPoolAsync(CancellationToken token)
    {
        bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(_despawnDelay), cancellationToken: token).SuppressCancellationThrow();
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
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}