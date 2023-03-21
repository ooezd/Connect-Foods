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

    public List<Item> selectedItems = new();
    public Item lastSelectedItem;

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
        if (lastSelectedItem._itemType.Equals(item._itemType))
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
}
