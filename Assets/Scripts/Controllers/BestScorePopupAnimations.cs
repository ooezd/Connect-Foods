using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestScorePopupAnimations : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    public void DisplayAnimation(Action onComplete)
    {
        AudioManager.Instance.PlayFx(FXClip.HighScore);
        transform.localScale = .4f * Vector3.one;
        transform.DOScale(1, .3f).SetEase(Ease.OutBack);
        foreach(var particle in particles)
        {
            particle.Play();
        }

        transform.DOScale(.2f, .5f)
            .SetEase(Ease.InBack)
            .SetDelay(1.5f).onComplete += () => { onComplete(); };
    }
}
