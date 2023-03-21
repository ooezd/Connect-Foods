using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : BaseState
{
    public override void EnterState(GameManager gameManager)
    {
        Debug.Log($"New state: {nameof(EndState)}");
        base.EnterState(gameManager);
    }

    public override void UpdateState()
    {

    }
    public override void ExitState()
    {
        
    }
}
