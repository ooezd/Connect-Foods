using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private ViewManager _viewManager;
    [SerializeField] BestScorePopupController bestScorePopup;

    void Start()
    {
        _viewManager = ViewManager.Instance;
        if (ProgressionManager.Instance.isHighScore)
        {
            bestScorePopup.gameObject.SetActive(true);
            bestScorePopup.Enable(OnHighScoreAnimationCompleted);
        }
        else if (ProgressionManager.Instance.showLevelsPopup)
        {
            ProgressionManager.Instance.showLevelsPopup = false;
            _viewManager.LoadPopup("LevelsPopup");
        }
        AudioManager.Instance.PlayMusic(MusicClip.MainMenu);
    }

    void OnHighScoreAnimationCompleted()
    {
        bestScorePopup.gameObject.SetActive(false);
        ProgressionManager.Instance.isHighScore = false;
        ProgressionManager.Instance.showLevelsPopup = false;
        _viewManager.LoadPopup("LevelsPopup");
    }

    public void OnLevelsButtonClick()
    {
        _viewManager.LoadPopup("LevelsPopup");
        AudioManager.Instance.PlayFx(FXClip.ButtonClick);
    }
}
