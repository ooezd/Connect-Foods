using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteContainer : MonoBehaviour
{
    public static SpriteContainer Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        foreach (var itemSprite in itemSprites)
        {
            itemSpriteDict.Add(itemSprite.type, itemSprite.sprite);
        }
    }
    [System.Serializable]
    public struct ItemSprite
    {
        public ItemType type;
        public Sprite sprite;
    }

    public ItemSprite[] itemSprites;

    private Dictionary<ItemType, Sprite> itemSpriteDict = new();

    void Start()
    {
        
    }

    public Sprite GetItemSprite(ItemType itemType)
    {
        return itemSpriteDict[itemType];
    }
}
