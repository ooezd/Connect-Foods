using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : BaseState
{
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log($"New state: {nameof(EndState)}");
        base.EnterState(gameManager);
        var gameResult = _gameManager.gameResult;
        _gameManager.gameUIController.DisplayEndGame(gameResult);
        var levelNumber = _gameManager.levelModel.levelData.levelNumber;
        var score = _gameManager.Point;
        if (gameResult.isPassed)
        {
            ProgressionManager.Instance.SavePassedLevel(levelNumber);
            ProgressionManager.Instance.lastScore = score;
            ProgressionManager.Instance.lastLevelNumber = levelNumber;
            var bestScore = ProgressionManager.Instance.GetBestScore(levelNumber);
            if(score > bestScore)
            {
                ProgressionManager.Instance.SaveBestScore(levelNumber, score);
            }
        }
    }

    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        
    }
}
