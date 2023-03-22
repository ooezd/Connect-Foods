using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    LevelModel _currentLevel;

    public static SessionManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void SetCurrentLevel(LevelModel currentLevel)
    {
        _currentLevel = currentLevel;
    }

    public LevelModel GetCurrentLevel()
    {
        return _currentLevel;
    }
}
