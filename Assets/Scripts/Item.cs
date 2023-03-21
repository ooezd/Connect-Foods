using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] SpriteRenderer itemSpriteRenderer;
    [SerializeField] SpriteRenderer tileSpriteRenderer;

    [HideInInspector] public ItemType _itemType;
    [HideInInspector] public Vector2 coordinate;

    ItemAnimations _itemAnimations;

    void Start()
    {
        TryGetComponent(out _itemAnimations);
    }
    public void Init(ItemType itemType, Vector2 coordinate)
    {
        _itemType = itemType;
        this.coordinate = coordinate;
        itemSpriteRenderer.sprite = SpriteContainer.Instance.GetItemSprite(_itemType);
    }
    public void OnSelected()
    {
        tileSpriteRenderer.color = Color.red;
    }
    public void OnDeselected()
    {
        tileSpriteRenderer.color = Color.white;
    }
    public void OnExplode(Action onComplete, int index)
    {
        _itemAnimations?.PlayExplodeAnimation(onComplete, index);
    }
}
public enum ItemType
{
    Pumpkin,
    Banana,
    Apple,
    Blueberry,
    DragonFruit
}