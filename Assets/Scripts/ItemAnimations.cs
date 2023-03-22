using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ItemAnimations : MonoBehaviour
{
    [SerializeField] Sprite[] tileSprites;
    [SerializeField] SpriteRenderer[] brokenTiles;
    [SerializeField] SpriteRenderer tileSpriteRenderer;
    [SerializeField] SpriteRenderer itemRenderer;

    [Header("Config")]
    [Range(0,10)]
    [SerializeField] private float speed;
    [Range(0, 1)]
    [SerializeField] private float randomize;
    [SerializeField] private float duration;
    [SerializeField] private float delayPerItemIndex;

    Sequence itemSequence;

    Vector3[] brokenTileInitialPositions;

    void Awake()
    {
        brokenTileInitialPositions = new Vector3[brokenTiles.Length];
        var brokenTileCount = brokenTiles.Length;
        
        for(int i =0; i < brokenTileCount; i++)
        {
            brokenTileInitialPositions[i] = brokenTiles[i].transform.position;
        }
    }
    public void PlayExplodeAnimation(Action onComplete, int index)
    {
        tileSpriteRenderer.enabled = false;
        foreach(SpriteRenderer brokenTile in brokenTiles)
        {
            var brokenTileTransform = brokenTile.transform;
            brokenTile.gameObject.SetActive(true);
            var directionFromCenter = brokenTileTransform.position - transform.position;
            directionFromCenter *= speed;

            brokenTile.DOFade(0, duration * .5f).SetDelay(.5f);
            brokenTileTransform.DOMove(transform.localPosition + directionFromCenter, duration)
                .SetEase(Ease.OutCirc);

            itemRenderer.transform.DOScale(1.3f, duration * .5f)
                .SetEase(Ease.InOutBack)
                .onComplete += () =>
                {
                    itemSequence.Append(itemRenderer.DOFade(0, duration * .25f));
                    itemSequence.Append(itemRenderer.transform.DOScale(.2f, duration * .25f));
                };
        }
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(duration + .2f);
        sequence.onComplete += () => { Reset(); onComplete();};
    }
    public void PlaySelectedAnimation()
    {
        tileSpriteRenderer.sprite = tileSprites[(int)TileState.Selected];
        transform.DOScale(1.02f, .2f);
    }
    public void PlayDeselectedAnimation()
    {
        tileSpriteRenderer.sprite = tileSprites[(int)TileState.Normal];
        transform.DOScale(1f, .2f);
    }
    void Reset()
    {
        itemRenderer.transform.localScale = Vector3.one;
        itemRenderer.color = Color.white;
        tileSpriteRenderer.enabled = true;
        tileSpriteRenderer.color = Color.white;
        tileSpriteRenderer.sprite = tileSprites[(int)TileState.Normal];
        transform.localScale = Vector3.one;

        for(int i = 0; i < brokenTiles.Length; i++)
        {
            brokenTiles[i].transform.position = brokenTileInitialPositions[i];
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
