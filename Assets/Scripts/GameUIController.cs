using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moveCountText;
    [SerializeField] Transform objectiveContainer;
    [SerializeField] GameObject objectiveItemPrefab;

    List<ObjectiveItem> objectiveItems = new();

    GameManager _gameManager;
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _gameManager.onMoveCountChanged += OnMoveCountChanged;
        _gameManager.onTargetObjectivesChanged += OnTargetObjectivesChanged;
    }

    private void OnMoveCountChanged(int newMoveCount)
    {
        moveCountText.text = newMoveCount.ToString();
    }

    private void OnTargetObjectivesChanged(List<TargetObjective> targetObjectives)
    {
        foreach(var item in objectiveItems)
        {
            Destroy(item);
        }
        objectiveItems.Clear();
        foreach(var targetObjective in targetObjectives)
        {
            var go = Instantiate(objectiveItemPrefab, objectiveContainer);
            ObjectiveItem objectiveItem = go.GetComponent<ObjectiveItem>();
            objectiveItem.Init(targetObjective);
        }
    }
}
