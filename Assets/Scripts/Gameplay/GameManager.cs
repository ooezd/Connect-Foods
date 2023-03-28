using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BaseState currentState;
    public IntroState introState = new();
    public PlayableState playableState = new();
    public ShuffleState shuffleState = new();
    public OutputState outputState = new();
    public EndState endState = new();

    [SerializeField] public GameUIController gameUIController;
    [SerializeField] private GameObject gridManagerPrefab;
    [SerializeField] private Camera uiCamera;

    [HideInInspector] public List<Item> selectedItems = new();
    [HideInInspector] public Item lastSelectedItem;

    [HideInInspector] public GridManager gridManager;
    [HideInInspector] public SessionManager sessionManager;
    [HideInInspector] public LevelModel levelModel;
    [HideInInspector] public GameResult gameResult;
    
    public List<TargetObjective> TargetObjectives {
        get { return _targetObjectives; }
        set 
        { 
            _targetObjectives = value;
            gameUIController.SetTargetObjectives(_targetObjectives);
            CreateDestroyedObjectsDict(_targetObjectives);
        }
    }
    private List<TargetObjective> _targetObjectives;

    public int MoveCount
    {
        get { return _moveCount; }
        set
        {
            _moveCount = Mathf.Max(0, value);
            onMoveCountChanged?.Invoke(_moveCount);
        }
    }
    private int _moveCount;
    public int Point
    {
        get { return _point; }
        set
        {
            _point = value;
            onPointChanged?.Invoke(_point);
        }
    }
    private int _point=0;
    public Dictionary<ItemType, int> _destroyedTargetObjectives = new();

    public event Action<int> onMoveCountChanged;
    public event Action<ItemType, int> onItemsDestroyed;
    public event Action<int> onPointChanged;

    public static GameManager Instance;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        var gridManagerObject = Instantiate(gridManagerPrefab);
        gridManager = gridManagerObject.GetComponent<GridManager>();
        gridManager.uiCamera = uiCamera;
        sessionManager = SessionManager.Instance;
    }
    void Start()
    {
        currentState = introState;
        currentState.EnterState(this);
    }
    void Update()
    {
        currentState?.UpdateState();
    }
    public void SwitchState(BaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState(this);
    }
    public bool IsValidType(Item item)
    {
        if(lastSelectedItem == null)
        {
            return true;
        }
        if (lastSelectedItem.itemType.Equals(item.itemType))
        {
            foreach (var selectedItem in selectedItems)
            {
                if (selectedItem.coordinate.Equals(item.coordinate))
                {
                    return false;
                }
            }
            if (lastSelectedItem.coordinate.Equals(item.coordinate))
            {
                return false;
            }
            var isAdjacentOrCross = Mathf.Abs(item.coordinate.x - lastSelectedItem.coordinate.x) <= 1
                && Mathf.Abs(item.coordinate.y - lastSelectedItem.coordinate.y) <= 1;
            return isAdjacentOrCross;
        }
        return false;
    }
    public void ItemsDestroyed(ItemType itemType, int count)
    {
        Point += count;
        if (_destroyedTargetObjectives.ContainsKey(itemType))
        {
            _destroyedTargetObjectives[itemType] += count;
        }
        onItemsDestroyed?.Invoke(itemType, count);
    }
    public void ItemDestroyed(Item item)
    {
        gridManager.RecycleItem(item);
    }
    public void ClearConnection()
    {
        selectedItems.Clear();
        lastSelectedItem = null;
    }
    void CreateDestroyedObjectsDict(List<TargetObjective> targetObjectives)
    {
        _destroyedTargetObjectives = new();
        foreach(var targetObjective in targetObjectives)
        {
            var itemType = Item.GetItemTypeFromName(targetObjective.name);
            _destroyedTargetObjectives.Add(itemType, 0);
        }
    }
}
