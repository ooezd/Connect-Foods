using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] Transform brokenTiles;
    [SerializeField] SpriteRenderer itemSpriteRenderer;
    [SerializeField] SpriteRenderer tileSpriteRenderer;

    [HideInInspector] public ItemType itemType;
    [HideInInspector] public Vector2 coordinate;

    ItemAnimations _itemAnimations;
    void Awake()
    {
        TryGetComponent(out _itemAnimations);
    }
    public void Init(ItemType itemType, Vector2 coordinate)
    {   
        this.itemType = itemType;
        this.coordinate = coordinate;
        itemSpriteRenderer.sprite = SpriteContainer.Instance.GetItemSprite(this.itemType);
        _itemAnimations?.PlayAppearAnimation();
    }
    public void OnSelected()
    {
        _itemAnimations.PlaySelectedAnimation();
    }
    public void OnDeselected()
    {
        _itemAnimations.PlayDeselectedAnimation();
    }
    public void OnExplode(Action<Item> onComplete, int index)
    {
        Action onAnimationComplete = () => { onComplete(this); };
        _itemAnimations?.PlayExplodeAnimation(onAnimationComplete, index);
        if(_itemAnimations == null)
        {
            onComplete(this);
        }
    }

    public static ItemType GetItemTypeFromName(string itemName)
    {
        switch (itemName)
        {
            case "Pumpkin":
                return ItemType.Pumpkin;
            case "Banana":
                return ItemType.Banana;
            case "Apple":
                return ItemType.Apple;
            case "Blueberry":
                return ItemType.Blueberry;
            case "DragonFruit":
                return ItemType.DragonFruit;
            default:
                Debug.LogError($"Item name mismatch: {itemName} couldn't be found.");
                return ItemType.None;
        }
    }
}
public enum ItemType
{
    Pumpkin,
    Banana,
    Apple,
    Blueberry,
    DragonFruit,
    None
}