using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected GameManager _gameManager;
    public virtual void EnterState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    public abstract void UpdateState();
    public abstract void ExitState();
}
