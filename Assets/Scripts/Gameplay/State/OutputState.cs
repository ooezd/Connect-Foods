using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputState : BaseState
{
    int connectionCount;
    int destroyedItemCount;
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log($"New state: {nameof(OutputState)}");
        base.EnterState(gameManager);
        destroyedItemCount = 0;
        connectionCount = 0;

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
        connectionCount = selectedItems.Count;
        _gameManager.ItemsDestroyed(selectedItems[0].itemType, connectionCount);
        _gameManager.MoveCount--;
        for (int i = 0; i < connectionCount; i++)
        {
            selectedItems[i].OnExplode(OnConnectionAnimationComplete, i);
        }
    }

    void OnConnectionAnimationComplete(Item item)
    {
        destroyedItemCount++;
        var gameResult = GetGameResult();
        _gameManager.gameResult = gameResult;
        _gameManager.ItemDestroyed(item);

        if (destroyedItemCount.Equals(connectionCount))
        {
            if (gameResult.isCompleted)
            {
                _gameManager.SwitchState(_gameManager.endState);
            }
            else
            {
                _gameManager.SwitchState(_gameManager.playableState);
            }
        }
    }
    public GameResult GetGameResult()
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
