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
        ClearConnection();
    }

    void HandleConnection()
    {
        var connection = _gameManager.selectedItems;
        var connectionCount = connection.Count;
        if (connectionCount >= 3)
        {
            for (int i = 0; i < connectionCount; i++)
            {
                connection[i].OnExplode(OnConnectionAnimationsComplete,i);
            }
        }
        else
        {
            for(int i = 0; i <connectionCount; i++)
            {
                connection[i].OnDeselected();
            }
            _gameManager.SwitchState(_gameManager.playableState);
        }
    }

    void OnConnectionAnimationsComplete()
    {
        if (isSwitching)
        {
            return;
        }
        isSwitching = true;
        _gameManager.SwitchState(_gameManager.playableState);
    }

    void ClearConnection()
    {
        _gameManager.selectedItems.Clear();
        _gameManager.lastSelectedItem = null;
    }
}
