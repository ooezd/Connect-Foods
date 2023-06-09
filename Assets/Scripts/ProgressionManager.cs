using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager
{
    const string LAST_PASSED_LEVEL_KEY = "last_passed_level";
    const string BEST_SCORES_KEY = "best_scores";

    private int lastPassedLevel = -1;
    private BestScores bestScores;
    public bool showLevelsPopup;
    public bool isHighScore;
    public int lastScore;
    public int lastLevelNumber;
    
    private static ProgressionManager _instance;
    public static ProgressionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ProgressionManager();
            }
            return _instance;
        }
    }
    private ProgressionManager()
    {
        InitData();
    }
    void InitData()
    {
        bestScores = LoadBestScores();
        lastPassedLevel = LoadLastPassedLevel();
    }

    public int GetBestScore(int levelNumber)
    {
        if(bestScores == null)
        {
            return 0;
        }
        if (!bestScores.bestScoresList.ContainsKey(levelNumber))
        {
            return 0;
        }
        return bestScores.bestScoresList[levelNumber];
    }
    public Dictionary<int,int> GetBestScores()
    {
        return bestScores.bestScoresList;
    }
    public void SaveBestScore(int levelNumber, int score)
    {
        if (bestScores.bestScoresList.ContainsKey(levelNumber))
        {
            var previousScore = bestScores.bestScoresList[levelNumber];
            if(score < previousScore)
            {
                return;
            }
            bestScores.bestScoresList[levelNumber] = score;
        }
        else
        {
            bestScores.bestScoresList.Add(levelNumber, score);
        }
        isHighScore = true;

        var bestScoreJson = JsonConvert.SerializeObject(bestScores, Formatting.Indented);
        PlayerPrefs.SetString(BEST_SCORES_KEY, bestScoreJson);
        PlayerPrefs.Save();
    }
    public void SavePassedLevel(int levelNumber)
    {
        showLevelsPopup = true;
        if(lastPassedLevel > levelNumber)
        {
            return;
        }

        lastPassedLevel = levelNumber;
        PlayerPrefs.SetInt(LAST_PASSED_LEVEL_KEY, lastPassedLevel);
        PlayerPrefs.Save();
    }
    public bool IsUnlocked(int levelNumber)
    {
        return levelNumber <= lastPassedLevel + 1;
    }
    int LoadLastPassedLevel()
    {
        return PlayerPrefs.GetInt(LAST_PASSED_LEVEL_KEY, 0);
    }
    BestScores LoadBestScores()
    {
        string bestScoreJson = PlayerPrefs.GetString(BEST_SCORES_KEY, JsonConvert.SerializeObject(new BestScores()));
        var bestScores = JsonConvert.DeserializeObject<BestScores>(bestScoreJson);
        return bestScores;
    }
    [Serializable]
    public class BestScores
    {
        public Dictionary<int, int> bestScoresList = new Dictionary<int, int>();
    }
}
