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

        if (_gameManager.IsValid(item))
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
        _gameManager.SwitchState(_gameManager.outputState);
    }
}
