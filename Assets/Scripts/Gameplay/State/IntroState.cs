using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : BaseState
{
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log($"New state: {nameof(IntroState)}");
        base.EnterState(gameManager);
    }

    public override void UpdateState()
    {
        _gameManager.SwitchState(_gameManager.playableState);
    }

    public override void ExitState()
    {
        
    }
}
