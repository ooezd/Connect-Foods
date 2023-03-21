using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject containerPrefab;
    Dictionary<Type, Pool> pools = new();
    Dictionary<Type, Transform> containers = new();
    public static PoolManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public Pool GetPool<TPool>(GameObject prefab, int initialAmount)
    {
        if (pools.ContainsKey(typeof(TPool)))
        {
            return pools[typeof(TPool)];
        }
        return CreatePool<TPool>(prefab, initialAmount);
    }
    Pool CreatePool<TPool>(GameObject prefab, int initialAmount)
    {
        var containerObject = Instantiate(containerPrefab);
        containerObject.name = typeof(TPool) + "Pool";
        containerObject.transform.SetParent(transform);
        Pool pool = new Pool(prefab, initialAmount, containerObject.transform);
        pools.Add(typeof(TPool), pool);
        containers.Add(typeof(TPool), containerObject.transform);

        return pool;
    }
    public void DestroyPool(Type poolType)
    {
        if (containers.ContainsKey(poolType))
        {
            Destroy(containers[poolType].gameObject);
            containers.Remove(poolType);
        }

        pools.Remove(poolType);
    }
}