using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleState : BaseState
{
    public override void EnterState(GameManager gameManager)
    {
        base.EnterState(gameManager);
        _gameManager.gridManager.Shuffle(OnShuffleComplete);
    }

    public override void UpdateState()
    {
        
    }
    public override void ExitState()
    {

    }
    
    void OnShuffleComplete()
    {
        _gameManager.SwitchState(_gameManager.playableState);
    }
}
