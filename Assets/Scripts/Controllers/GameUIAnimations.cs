using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class GameUIAnimations : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] ParticleSystem[] matchParticles;
    public void PlayMatchAnimation()
    {
        if(matchParticles.Length<= 0)
        {
            return;
        }
        int randomIndex = Random.Range(0, matchParticles.Length);
        matchParticles[randomIndex].Play();
        AudioManager.Instance.PlayFx(FXClip.Match);
    }
    public void PlayEndAnimations(bool isPassed, Action onComplete)
    {
        var audioType = isPassed ? FXClip.Win : FXClip.Lose;
        AudioManager.Instance.PlayFx(audioType);
        mainText.text = isPassed ? "Success!" : "Failed!";
        mainText.gameObject.SetActive(true);
        mainText.transform.localScale = Vector3.one * .2f;
        mainText.transform.DOScale(1, .3f)
            .SetEase(Ease.InCubic)
            .onComplete += () =>
            {
                mainText.transform.DOPunchScale(1.1f * Vector3.one, .2f)
                .onComplete += ()=>
                {
                    mainText.transform.DOShakeRotation(1f, 70, 4, 50)
                    .SetLoops(1,LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .onComplete += ()=>
                    {
                        mainText.transform.DOScale(0, .3f).SetDelay(.5f)
                        .onComplete += () =>
                        {
                            onComplete();
                        };
                    };
                };
            };
    }
}
