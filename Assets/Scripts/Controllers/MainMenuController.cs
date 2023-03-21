using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private ViewManager _viewManager;

    void Awake()
    {
        _viewManager = ViewManager.Instance;
    }

    public void OnLevelsButtonClick()
    {
        _viewManager.LoadPopup("LevelsPopup");
    }
}
