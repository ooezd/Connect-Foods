using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moveCountText;
    [SerializeField] Transform objectiveContainer;
    [SerializeField] GameObject objectiveItemPrefab;
    [SerializeField] TextMeshProUGUI score;

    List<ObjectiveItem> objectiveItems = new();

    GameManager _gameManager;
    GameUIAnimations _animations;
    Canvas _canvas;
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _gameManager.onMoveCountChanged += OnMoveCountChanged;
        _gameManager.onPointChanged += OnUpdateScore;
        _canvas = GetComponent<Canvas>();
        TryGetComponent(out _animations);
    }
    void Start()
    {
        _canvas.planeDistance = _gameManager.gridManager._cameraSize;
        AudioManager.Instance.PlayMusic(MusicClip.GamePlay);
    }
    public void SetTargetObjectives(List<TargetObjective> targetObjectives)
    {
        foreach (var item in objectiveItems)
        {
            Destroy(item);
        }
        objectiveItems.Clear();
        foreach (var targetObjective in targetObjectives)
        {
            var go = Instantiate(objectiveItemPrefab, objectiveContainer);
            ObjectiveItem objectiveItem = go.GetComponent<ObjectiveItem>();
            objectiveItem.Init(targetObjective);
            objectiveItems.Add(objectiveItem);
        }
    }
    public void DisplayEndGame(GameResult gameResult)
    {
        _animations?.PlayEndAnimations(gameResult.isPassed, ReturnToMenu);
    }
    public void ReturnToMenu()
    {
        ViewManager.Instance.LoadScene(0);
    }
    public void OnBackButtonClicked()
    {
        Debug.Log("BackButtonClicked");
        ViewManager.Instance.LoadScene(0);
    }
    private void OnUpdateScore(int newScore)
    {
        score.text = newScore.ToString();
        _animations?.PlayMatchAnimation();
    }

    private void OnMoveCountChanged(int newMoveCount)
    {
        moveCountText.text = newMoveCount.ToString();
    }
    void OnDestroy()
    {
        _gameManager.onMoveCountChanged -= OnMoveCountChanged;
        _gameManager.onPointChanged -= OnUpdateScore;
    }
}
