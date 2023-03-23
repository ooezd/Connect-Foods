using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableState : BaseState
{
    InputProvider _inputProvider;
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log($"New state: {nameof(PlayableState)}");
        base.EnterState(gameManager);

        _inputProvider = InputProvider.Instance;
        _inputProvider.selectionStarted += StartSelection;
        _inputProvider.selectionContinue += ContinueSelection;
        _inputProvider.releaseSelection += ReleaseSelection;
        _inputProvider.SetActive(true);

        var seq = DOTween.Sequence();
        seq.AppendInterval(.1f).onComplete += () =>
        {
            if (!_gameManager.gridManager.HasPotentialMatch())
            {
                _gameManager.SwitchState(_gameManager.shuffleState);
            }
        };
    }

    public override void UpdateState()
    {
        
    }
    public override void ExitState()
    {
        _inputProvider.selectionStarted -= StartSelection;
        _inputProvider.selectionContinue -= ContinueSelection;
        _inputProvider.releaseSelection -= ReleaseSelection;
        _inputProvider.SetActive(false);
    }

    private void ContinueSelection(Item item)
    {
        if (_gameManager.lastSelectedItem == null)
            return;

        if (_gameManager.IsValidType(item))
        {
            item.OnSelected();
            _gameManager.selectedItems.Add(item);
            _gameManager.lastSelectedItem = item;
        }
    }

    private void StartSelection(Item item)
    {
        item.OnSelected();
        _gameManager.selectedItems.Add(item);
        _gameManager.lastSelectedItem = item;
    }
    private void ReleaseSelection()
    {
        var selectedItems = _gameManager.selectedItems;
        var connectionCount = selectedItems.Count;
        if (HasEnoughConnections(connectionCount))
        {
            _gameManager.SwitchState(_gameManager.outputState);
        }
        else
        {
            for (int i = 0; i < connectionCount; i++)
            {
                selectedItems[i].OnDeselected();
            }
            _gameManager.ClearConnection();
        }
    }
    bool HasEnoughConnections(int connectionCount)
    {
        return connectionCount >= 3;
    }
}
