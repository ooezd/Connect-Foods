using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] GameObject _poolManagerPrefab;
    [SerializeField] GameObject _sessionManagerPrefab;
    [SerializeField] GameObject _audioManagerPrefab;

    void Awake()
    {
        Application.targetFrameRate = 120;
        if(PoolManager.Instance == null)
        {
            Instantiate(_poolManagerPrefab);
            Instantiate(_sessionManagerPrefab);
            Instantiate(_audioManagerPrefab);
        }
    }
}
