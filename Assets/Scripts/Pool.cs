using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool
{
    private List<GameObject> pooledList = new List<GameObject>();
    private List<GameObject> busyList = new List<GameObject>();

    private GameObject _prefab;
    private Transform _container;
    public Pool(GameObject prefab, int initialAmount, Transform container)
    {
        _container = container;
        _prefab = prefab;
        Initialize(initialAmount);
    }
    public void Initialize(int amount)
    {
        SpawnNewObjects(amount);
    }
    GameObject SpawnNewObject()
    {
        var go = GameObject.Instantiate(_prefab);
        go.transform.SetParent(_container);
        go.SetActive(false);
        pooledList.Add(go);

        return go;
    }
    void SpawnNewObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnNewObject();
        }
    }
    public GameObject GetFromPool()
    {
        GameObject go;
        if(pooledList.Count > 0)
        {
            go = pooledList.First();
            pooledList.Remove(go);
            busyList.Add(go);
            go.SetActive(true);

            return go;
        }
        go = SpawnNewObject();
        pooledList.Remove(go);
        busyList.Add(go);

        return SpawnNewObject();
    }
    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
        busyList.Remove(go);
        pooledList.Add(go);
        go.transform.SetParent(_container);
    }
}
