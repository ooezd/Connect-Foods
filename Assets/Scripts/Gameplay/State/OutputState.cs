using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputState : BaseState
{
    Sequence scoreSequence;
    Sequence failSequence;

    bool isSwitching;
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log($"New state: {nameof(OutputState)}");
        base.EnterState(gameManager);
        isSwitching = false;

        HandleConnection();
    }

    public override void UpdateState()
    {
        
    }
    public override void ExitState()
    {
        _gameManager.ClearConnection();
    }

    void HandleConnection()
    {
        var selectedItems = _gameManager.selectedItems;
        var connectionCount = selectedItems.Count;
        _gameManager.ItemsDestroyed(selectedItems[0].itemType, connectionCount);
        _gameManager.MoveCount--;
        for (int i = 0; i < connectionCount; i++)
        {
            selectedItems[i].OnExplode(OnConnectionAnimationsComplete, i);
        }
    }

    void OnConnectionAnimationsComplete(Item item)
    {
        var gameResult = CheckGameCompleted();
        _gameManager.gameResult = gameResult;
        _gameManager.ItemDestroyed(item);
        if (isSwitching)
        {
            return;
        }
        isSwitching = true;
        if (gameResult.isCompleted)
        {
            _gameManager.SwitchState(_gameManager.endState);
        }
        else
        {
            _gameManager.SwitchState(_gameManager.playableState);
        }
    }
    public GameResult CheckGameCompleted()
    {
        GameResult gameResult;
        foreach(var targetObjective in _gameManager.TargetObjectives)
        {
            ItemType itemType = Item.GetItemTypeFromName(targetObjective.name);
            var hasAnyMove = _gameManager.MoveCount > 0;
            if (!_gameManager._destroyedTargetObjectives.ContainsKey(itemType))
            {
                Debug.LogError($"Item type of {nameof(itemType)} doesn't exist in destroyedTargetObjectives list.");
            }

            var isObjectiveCompleted = _gameManager._destroyedTargetObjectives[itemType] >= targetObjective.count;
            if (!isObjectiveCompleted)
            {
                gameResult = new GameResult()
                {
                    isCompleted = !hasAnyMove,
                    isPassed = false
                };
                return gameResult;
            }
        }
        gameResult = new GameResult()
        {
            isCompleted = true,
            isPassed = true
        };
        return gameResult;
    }
}
public struct GameResult
{
    public bool isCompleted;
    public bool isPassed;
}
