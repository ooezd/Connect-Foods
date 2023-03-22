using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveItem : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Image tileImage;
    [SerializeField] TextMeshProUGUI countText;

    ItemType _itemType;
    int _count;
    public void Init(TargetObjective targetObjective)
    {
        GameManager.Instance.onItemsDestroyed += OnItemsDestroyed;
        switch (targetObjective.name)
        {
            case "Pumpkin":
                _itemType = ItemType.Pumpkin;
                break;
            case "Banana":
                _itemType = ItemType.Banana;
                break;
            case "Apple":
                _itemType = ItemType.Apple;
                break;
            case "Blueberry":
                _itemType = ItemType.Blueberry;
                break;
            case "DragonFruit":
                _itemType = ItemType.DragonFruit;
                break;
            default:
                Debug.LogError($"Item name mismatch: {targetObjective.name} couldn't be found.");
                return;
        }
        itemImage.sprite = SpriteContainer.Instance.GetItemSprite(_itemType);
        _count = targetObjective.count;
        UpdateCount(_count);
    }

    private void OnItemsDestroyed(ItemType itemType, int destroyedCount)
    {
        if (!itemType.Equals(_itemType))
        {
            return;
        }

        var newCount = Mathf.Max(0, _count - destroyedCount);
        UpdateCount(newCount);
    }

    public void UpdateCount(int count)
    {
        _count = count;
        countText.text = _count.ToString();
    }
    void OnDestroy()
    {
        GameManager.Instance.onItemsDestroyed -= OnItemsDestroyed;
    }
}
