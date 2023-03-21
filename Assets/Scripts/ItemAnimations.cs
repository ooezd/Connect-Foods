using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ItemAnimations : MonoBehaviour
{
    [SerializeField] Transform brokenTiles;
    [SerializeField] SpriteRenderer tileRenderer;
    [SerializeField] SpriteRenderer itemRenderer;

    [Header("Config")]
    [Range(0,10)]
    [SerializeField] private float speed;
    [Range(0, 1)]
    [SerializeField] private float randomize;
    [SerializeField] private float duration;
    [SerializeField] private float delayPerItemIndex;

    public void PlayExplodeAnimation(Action onComplete, int index)
    {
        tileRenderer.enabled = false;
        foreach(Transform brokenTile in brokenTiles)
        {
            brokenTile.gameObject.SetActive(true);
            var directionFromCenter = brokenTile.position - brokenTiles.position;
            directionFromCenter *= speed;

            brokenTile.DOMove(transform.localPosition + directionFromCenter, duration)
                .SetEase(Ease.OutSine)
                .SetDelay(index * delayPerItemIndex)
                .onComplete += () => { onComplete(); gameObject.SetActive(false); };

        }
    }
}
