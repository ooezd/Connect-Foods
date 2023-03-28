using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class ItemAnimations : MonoBehaviour
{
    [SerializeField] Sprite[] tileSprites;
    [SerializeField] SpriteRenderer[] brokenTiles;
    [SerializeField] SpriteRenderer tileRenderer;
    [SerializeField] SpriteRenderer itemRenderer;

    [Header("Config")]
    [Range(0,10)]
    [SerializeField] private float speed;
    [Range(0, 1)]
    [SerializeField] private float randomize;
    [SerializeField] private float duration;
    [SerializeField] private float delayPerItemIndex;
    Vector3[] brokenTileInitialOffsets;

    void Awake()
    {
        brokenTileInitialOffsets = new Vector3[brokenTiles.Length];
        var brokenTileCount = brokenTiles.Length;
        
        for(int i =0; i < brokenTileCount; i++)
        {
            brokenTileInitialOffsets[i] = brokenTiles[i].transform.position - transform.position;
        }
    }
    public void PlayAppearAnimation()
    {
        Reset();
        itemRenderer.DOColor(Color.white, duration * .35f);
        tileRenderer.DOColor(Color.white, duration * .35f);
        tileRenderer.transform.DOScale(1, duration * .35f).SetEase(Ease.OutBack);
        transform.DOScale(1f, duration * .35f).SetEase(Ease.OutBack);
        itemRenderer.transform.DOScale(1f, duration * .35f).SetEase(Ease.OutBack).SetDelay(duration*.06f);
    }
    public void PlayExplodeAnimation(Action onComplete, int index)
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(index * .06f).onComplete += ()=>
        {
            tileRenderer.enabled = false;
            AudioManager.Instance.PlayFx(FXClip.TileRemove,.6f);
            foreach (SpriteRenderer brokenTile in brokenTiles)
            {
                var brokenTileTransform = brokenTile.transform;
                brokenTile.gameObject.SetActive(true);
                var directionFromCenter = brokenTileTransform.position - transform.position;
                
                var brokenTileSpeed = directionFromCenter.normalized * speed;

                var randomVector = new Vector3(UnityEngine.Random.Range(-randomize, randomize), UnityEngine.Random.Range(-randomize, randomize));
                brokenTile.DOFade(0, duration * .5f).SetDelay(.5f);
                brokenTileTransform.DOMove(transform.localPosition + brokenTileSpeed + randomVector, duration)
                    .SetEase(Ease.InOutSine);
            }
            itemRenderer.transform.DOScale(1.3f, duration * .4f)
                .SetEase(Ease.InOutBack);

            itemRenderer.DOFade(0, duration * .35f);
            itemRenderer.transform.DOScale(0f, duration * .35f).SetEase(Ease.InBack);
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(duration + .5f);
            sequence.onComplete += () => { Reset(); onComplete(); };
        };
    }
    public void PlaySelectedAnimation()
    {
        AudioManager.Instance.PlayFx(FXClip.TileSelect);
        tileRenderer.sprite = tileSprites[(int)TileState.Selected];
        transform.DOScale(1.1f, .2f);
    }
    public void PlayDeselectedAnimation()
    {
        tileRenderer.sprite = tileSprites[(int)TileState.Normal];
        transform.DOScale(1f, .2f);
    }
    void Reset()
    {
        itemRenderer.color = Color.clear;
        itemRenderer.transform.localScale = Vector3.zero;
        tileRenderer.color = Color.clear;
        tileRenderer.transform.localScale = Vector3.zero;
        tileRenderer.enabled = true;
        tileRenderer.sprite = tileSprites[(int)TileState.Normal];
        transform.localScale = Vector3.one;

        ResetBrokenTiles();
    }
    void ResetBrokenTiles()
    {
        for (int i = 0; i < brokenTiles.Length; i++)
        {
            brokenTiles[i].transform.position = transform.position + brokenTileInitialOffsets[i];
            brokenTiles[i].color = Color.white;
            brokenTiles[i].gameObject.SetActive(false);
        }
    }

    enum TileState
    {
        Normal=0,
        Selected=1,
    }
}
