using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveConfigSO _waveConfig;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _timeBetweenWaves = 3f;

    private int _currentWave = 0;
    private int _activeEnemies = 0;
    private float _waveTimer = 0f;
    private bool _isWaitingForNextWave = true;

    private void OnEnable()
    {
        EventBus<EnemyDeathEvent>.OnEvent += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        EventBus<EnemyDeathEvent>.OnEvent -= HandleEnemyDeath;
    }

    private void Start()
    {
        StartNextWave();
    }

    private void Update()
    {
        if (!_isWaitingForNextWave) return;

        _waveTimer -= Time.deltaTime;
        if (_waveTimer <= 0)
        {
            _isWaitingForNextWave = false;
            SpawnWave();
        }
    }

    private void StartNextWave()
    {
        _currentWave++;
        _waveTimer = _timeBetweenWaves;
        _isWaitingForNextWave = true;
        EventBus<WaveStartedEvent>.Raise(new WaveStartedEvent { WaveNumber = _currentWave });
    }

    private void SpawnWave()
    {
        if (_waveConfig == null || _waveConfig.BotWaves == null) return;

        foreach (var botWave in _waveConfig.BotWaves)
        {
            if (botWave.BotPrefab == null) continue;

            int spawnCount = Random.Range(botWave.MinInWave, botWave.MaxInWave + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy(botWave.BotPrefab);
            }
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        GameObject enemyObj = ObjectPoolingManager.Instance.Get(prefab);
        
        enemyObj.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        
        if (enemyObj.TryGetComponent(out AI_ScriptManager aiManager))
        {
            if (aiManager.NavAgent != null)
            {
                aiManager.NavAgent.Warp(spawnPoint.position);
            }
            aiManager.ActivateAI();
        }

        _activeEnemies++;
        EventBus<EnemySpawnedEvent>.Raise(new EnemySpawnedEvent());
    }

    private void HandleEnemyDeath(EnemyDeathEvent data)
    {
        _activeEnemies--;
        if (_activeEnemies <= 0 && !_isWaitingForNextWave)
        {
            StartNextWave();
        }
    }
}