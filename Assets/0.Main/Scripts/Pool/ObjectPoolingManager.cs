using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PoolDObject
{
    public GameObject Prefab;
    public int PreSpawnCount;
}

public class ObjectPoolingManager : MonoBehaviour, IObjectPool
{   
    public static ObjectPoolingManager Instance { get; private set; }

    public List<PoolDObject> PoolObjects;

    private readonly Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {   

        Instance = this;
        foreach (var poolObject in PoolObjects)
        {
            var pool = new Queue<GameObject>(poolObject.PreSpawnCount);
            _pools[poolObject.Prefab] = pool;

            for (int i = 0; i < poolObject.PreSpawnCount; i++)
            {
                CreateNewObject(poolObject.Prefab);
            }
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        
        _instanceToPrefab[obj] = prefab;

        if (obj.TryGetComponent(out IPoolObject poolObj))
        {
            poolObj.SetPool(this);
        }
        
        _pools[prefab].Enqueue(obj);
    }

    public GameObject Get(GameObject prefab)
    {
        if (!_pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            _pools[prefab] = pool;
        }

        if (pool.Count == 0)
        {
            CreateNewObject(prefab);
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        if (obj.TryGetComponent(out IPoolObject pObj))
        {
            pObj.OnSpawn();
        }

        return obj;
    }

    public void Return(GameObject obj)
    {
        if (!_instanceToPrefab.TryGetValue(obj, out GameObject prefab))
        {
            Destroy(obj);
            return;
        }

        if (obj.TryGetComponent(out IPoolObject pObj))
        {
            pObj.OnDespawn();
        }
        
        obj.SetActive(false);
        _pools[prefab].Enqueue(obj);
    }
}