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
    [SerializeField] Color unlockedRowColor;
    [SerializeField] Color lockedRowColor;
    [SerializeField] Image rowImage;

    private const string BEST_SCORE_PREFIX = "Best score: ";
    private const string LEVEL_PREFIX = "Level <size=40>";
    private const string LEVEL_SUFFIX = "</size>";
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
        levelLabel.text = LEVEL_PREFIX + levelData.levelNumber.ToString() + LEVEL_SUFFIX;
        bestScoreText.gameObject.SetActive(bestScore > 0);
        bestScoreText.text = BEST_SCORE_PREFIX + _model.bestScore;

        PreparePlayButton(_model.isUnlocked);
    }
    void PreparePlayButton(bool isUnlocked)
    {
        playButton.image.enabled = isUnlocked;
        playButtonLockImage.enabled = !isUnlocked;
        playButton.interactable = isUnlocked;
        playButtonText.enabled = isUnlocked;
        rowImage.color = isUnlocked ? unlockedRowColor : lockedRowColor;
    }

    public void OnPlayButtonClick()
    {
        AudioManager.Instance.PlayFx(FXClip.ButtonClick);
        if (!_model.isUnlocked)
        {
            return;
        }

        _onClicked?.Invoke(_model);
    }
}
