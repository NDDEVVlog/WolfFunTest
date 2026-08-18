using UnityEngine;

public interface IPoolObject
{
    void OnSpawn();
    void OnDespawn();
    void SetPool(IObjectPool pool);
}

public interface IObjectPool
{
    GameObject Get(GameObject prefab);
    void Return(GameObject obj);
}