using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public event Action<Item> selectionStarted;
    public event Action<Item> selectionContinue;
    public event Action releaseSelection;
    public static InputProvider Instance;
    private bool _isActive;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Update()
    {
        if (!_isActive)
        {
            return;
        }
        HandleInputs();
    }
    private void HandleInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                var hitTransform = hit.transform;
                if (hitTransform.CompareTag("Item"))
                {
                    if (hitTransform.TryGetComponent(out Item item))
                    {
                        selectionStarted?.Invoke(item);
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                var hitTransform = hit.transform;
                if (hitTransform.CompareTag("Item"))
                {
                    if (hit.transform.TryGetComponent(out Item item))
                    {
                        selectionContinue?.Invoke(item);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            releaseSelection?.Invoke();
        }
    }
    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }
}
