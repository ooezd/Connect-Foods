using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class BestScorePopupController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;

    const string LEVEL_PREFIX = "Level ";

    BestScorePopupAnimations _animations;

    void Awake()
    {
        TryGetComponent(out _animations);
    }

    public void Enable(Action onAnimationComplete)
    {
        var levelNumber = ProgressionManager.Instance.lastLevelNumber;
        var score = ProgressionManager.Instance.lastScore;

        levelText.text = LEVEL_PREFIX + levelNumber.ToString();
        scoreText.text = score.ToString();

        _animations?.DisplayAnimation(onAnimationComplete);
    }
}
