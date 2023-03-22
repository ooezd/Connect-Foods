using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IntroState : BaseState
{
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log($"New state: {nameof(IntroState)}");
        base.EnterState(gameManager);

        var levelModel = _gameManager.sessionManager.GetCurrentLevel();
        _gameManager.levelModel = levelModel;
        _gameManager.MoveCount = levelModel.levelData.totalMove;
        _gameManager.gridManager.Init(levelModel.levelData.row, levelModel.levelData.column);
        _gameManager.TargetObjectives = levelModel.levelData.targetObjectives.ToList();
    }

    public override void UpdateState()
    {
        _gameManager.SwitchState(_gameManager.playableState);
    }

    public override void ExitState()
    {
        
    }
}
