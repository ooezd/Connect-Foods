using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] SpriteRenderer background;

    [SerializeField] float referenceSize;
    [SerializeField] float backgroundPreferredSize;
    public Camera uiCamera;

    int _xDim;
    int _yDim;
    float offset = .75f;
    Camera _camera;
    Vector3 _centerPos;
    [HideInInspector]public float _cameraSize;

    Item[,] items;
    Vector2[,] itemPositions;
    ItemType[,] itemTypes;

    Pool _itemPool;

    public static GridManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void Init(int xDim, int yDim)
    {
        _xDim = xDim;
        _yDim = yDim;

        _itemPool = PoolManager.Instance.GetPool<Item>(itemPrefab, _xDim * _yDim + 20);

        items = new Item[_xDim, _yDim];
        itemPositions = new Vector2[_xDim, _yDim];
        itemTypes = new ItemType[_xDim, _yDim];
        _camera = Camera.main;
        SetCameraPosition();
        SetCameraSize();
        CreateGrid();
        Physics2D.queriesHitTriggers = true;
    }
    void SetCameraSize()
    {
        var camSize = _camera.orthographicSize;
        var ySize = camSize * 2;
        var xSize = ySize * (float)Screen.width / (float)Screen.height;
        var screenSize = (camSize * _xDim / xSize) -(.2f * _xDim);
        _cameraSize = screenSize;
        _camera.orthographicSize = screenSize;
        uiCamera.orthographicSize = screenSize;
        background.transform.localScale = (screenSize/referenceSize) * backgroundPreferredSize * Vector3.one;
    }
    void CreateGrid()
    {
        for(int i = 0; i < _xDim; i++)
        {
            for(int j = 0; j < _yDim; j++)
            {
                var coordinate = new Vector2(i, j);
                var spawnPos = coordinate * offset;
                itemPositions[i,j] = spawnPos;
                CreateNewItem(i, j);
            }
        }
    }
    void SetCameraPosition()
    {
        var xPos = ((_xDim - 1) * offset) / 2;
        var yPos = ((_yDim - 1) * offset) / 2 + .5f;
        _centerPos = new Vector3(xPos, yPos, -10f);
        _camera.transform.position = _centerPos;
        uiCamera.transform.position = _centerPos;
        background.transform.position = new Vector3(xPos, yPos);
    }
    ItemType GetRandomItemType()
    {
        var index = Random.Range(0, sizeof(ItemType) + 1);
        return (ItemType)index;
    }
    public void CreateNewItem(int xCoordinate, int yCoordinate)
    {
        var spawnPos = itemPositions[xCoordinate, yCoordinate];

        var itemObject = _itemPool.GetFromPool();
        itemObject.SetActive(true);
        itemObject.transform.SetParent(transform);
        itemObject.transform.position = spawnPos;

        var item = itemObject.GetComponent<Item>();
        var itemType = GetRandomItemType();
        item.Init(itemType, new Vector2(xCoordinate, yCoordinate));
        items[xCoordinate, yCoordinate] = item;
        itemTypes[xCoordinate, yCoordinate] = itemType;
    }
    public void RecycleItem(Item item)
    {
        Debug.Log($"GridManager ItemDestroyed: {item.coordinate}");
        _itemPool.ReturnToPool(item.gameObject);
        CreateNewItem((int)item.coordinate.x, (int)item.coordinate.y);
    }
    public void Shuffle(Action onComplete)
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(2).onComplete += () =>
        {
            Debug.Log("SHUFFLE");

            for (int i = 0; i < _xDim; i++)
            {
                for (int j = 0; j < _yDim; j++)
                {
                    RecycleItem(items[i, j]);
                }
            }

            onComplete.Invoke();
        };
    }

    #region DFS
    bool[,] visitedMatrix;
    public bool HasPotentialMatch()
    {
        visitedMatrix = new bool[_xDim, _yDim];

        for(int row = 0; row < _xDim; row++)
        {
            for(int column = 0; column < _yDim; column++)
            {
                if (!itemTypes[row, column].Equals(ItemType.None))
                {
                    var connectionCount = 0;
                    if (IsValidConnection(row, column, ++connectionCount))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool IsValidConnection(int xPos, int yPos, int connectionCount)
    {
        if (connectionCount >= 3)
        {
            return true;
        }
        if (visitedMatrix[xPos, yPos])
        {
            return false;
        }
        visitedMatrix[xPos, yPos] = true;

        if (xPos < 0 || yPos < 0 || xPos >= itemTypes.GetLength(0) || yPos >= itemTypes.GetLength(1))
        {
            return false;
        }

        for(int x = xPos - 1; x <=xPos+1; x++)
        {
            for(int y = yPos -1; y <= yPos + 1; y++)
            {
                if(x!=xPos || y != yPos)
                {
                    if (x < 0 || y < 0 || x >= itemTypes.GetLength(0) || y >= itemTypes.GetLength(1))
                    {
                        continue;
                    }
                    if (itemTypes[x, y].Equals(itemTypes[xPos, yPos]))
                    {
                        if (visitedMatrix[x, y])
                        {
                            continue;
                        }

                        connectionCount += 1;
                        if(connectionCount >= 3)
                        {
                            return true;
                        }

                        return IsValidConnection(x, y, connectionCount);
                    }
                }
            }
        }
        return false;
    }
    #endregion
}
