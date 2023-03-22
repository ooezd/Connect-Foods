using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BaseState currentState;
    public IntroState introState = new();
    public PlayableState playableState = new();
    public OutputState outputState = new();
    public EndState endState = new();

    [SerializeField] public GameUIController gameUIController;
    [SerializeField] private GameObject gridManagerPrefab;

    [HideInInspector] public List<Item> selectedItems = new();
    [HideInInspector] public Item lastSelectedItem;

    [HideInInspector] public GridManager gridManager;
    [HideInInspector] public SessionManager sessionManager;
    [HideInInspector] public LevelModel levelModel;
    
    public List<TargetObjective> TargetObjectives {
        get { return _targetObjectives; }
        set 
        { 
            _targetObjectives = value;
            onTargetObjectivesChanged?.Invoke(_targetObjectives);
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

    public event Action<int> onMoveCountChanged;
    public event Action<List<TargetObjective>> onTargetObjectivesChanged;
    public event Action<ItemType, int> onItemsDestroyed;

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
        sessionManager = SessionManager.Instance;
    }

    void Start()
    {
        currentState = introState;
        currentState.EnterState(this);
    }
    void Update()
    {
        currentState.UpdateState();
    }
    public void SwitchState(BaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState(this);
    }
    
    public bool IsValid(Item item)
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
        onItemsDestroyed?.Invoke(itemType, count);
    }
    public void ItemDestroyed(Item item)
    {
        gridManager.ItemDestroyed(item);
    }
}
