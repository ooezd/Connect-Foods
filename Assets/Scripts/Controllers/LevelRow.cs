using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelLabel;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] Button playButton;
    [SerializeField] TextMeshProUGUI playButtonText;
    [SerializeField] Image playButtonLockImage;

    private const string BEST_SCORE_PREFIX = "Best score: ";
    private const string LEVEL_PREFIX = "Level ";
    private LevelModel _model;
    private Action<LevelModel> _onClicked;

    public void SetData(LevelModel model, Action<LevelModel> onClicked)
    {
        _onClicked = onClicked;
        _model = model;
        BindData();
    }
    void BindData()
    {
        var levelData = _model.levelData;
        var bestScore = _model.bestScore;
        levelLabel.text = LEVEL_PREFIX + levelData.levelNumber.ToString();
        if(bestScore<= 0)
        {
            bestScoreText.enabled = false;
        }
        bestScoreText.text = BEST_SCORE_PREFIX + _model.bestScore;
        PreparePlayButton(_model.isUnlocked);
    }
    void PreparePlayButton(bool isUnlocked)
    {
        playButton.interactable = _model.isUnlocked;
        playButton.image.color = isUnlocked ? Color.green : Color.grey;

        playButtonLockImage.enabled = !isUnlocked;
        playButtonText.enabled = isUnlocked;
    }

    public void OnPlayButtonClick()
    {
        if (!_model.isUnlocked)
        {
            return;
        }

        _onClicked?.Invoke(_model);
    }
}
