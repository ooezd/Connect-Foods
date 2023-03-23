using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class LevelReader : MonoBehaviour
{
    [SerializeField] TextAsset levelsJSON;
    LevelsList levels;

    public static LevelReader Instance;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        LoadLevels();
    }

    void LoadLevels()
    {
        levels = JsonConvert.DeserializeObject<LevelsList>(levelsJSON.text);
    }

    public LevelData GetLevelData(int levelNumber)
    {
        if (levels.levels.Length < levelNumber)
        {
            Debug.LogError($"No data available for level number {levelNumber}");
            return null;
        }

        var levelIndex = levelNumber - 1;
        return levels.levels[levelIndex];
    }
    public LevelData[] GetLevelsList()
    {
        return levels.levels;
    }
    public List<LevelModel> GetLevelModels()
    {
        var modelsList = new List<LevelModel>();
        foreach(var levelData in levels.levels)
        {
            var levelNumber = levelData.levelNumber;
            var bestScore = ProgressionManager.Instance.GetBestScore(levelNumber);
            var isUnlocked = ProgressionManager.Instance.IsUnlocked(levelNumber);

            var model = new LevelModel()
            {
                levelData = levelData,
                bestScore = bestScore,
                isUnlocked = isUnlocked
            };
            modelsList.Add(model);
        }

        return modelsList;
    }
}

[Serializable]
public class LevelsList
{
    public LevelData[] levels;
}

[Serializable]
public class LevelData
{
    public int levelNumber;
    public int totalMove;
    public int row;
    public int column;
    public TargetObjective[] targetObjectives;
}

[Serializable]
public class TargetObjective
{
    public int count;
    public string name;
}

public struct LevelModel
{
    public LevelData levelData;
    public int bestScore;
    public bool isUnlocked;
}