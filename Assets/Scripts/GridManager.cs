using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] SpriteRenderer background;

    [SerializeField] float referenceSize;
    [SerializeField] float backgroundPreferredSize;

    int _xDim;
    int _yDim;
    float offset = .75f;
    Camera _camera;

    Item[,] items;
    Vector2[,] itemPositions;

    PoolManager _poolManager;
    Pool _itemPool;

    public static GridManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        _poolManager = PoolManager.Instance;
    }

    public void Init(int xDim, int yDim)
    {

        _xDim = xDim;
        _yDim = yDim;
        
        if (_itemPool == null)
        {
            _itemPool = _poolManager.GetPool<Item>(itemPrefab, _xDim * _yDim + 10);
        }

        items = new Item[_xDim, yDim];
        itemPositions = new Vector2[_xDim, yDim];
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
        _camera.orthographicSize = screenSize;
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
        _camera.transform.position = new Vector3(xPos, yPos, -10f);
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
        itemObject.transform.SetParent(transform);
        itemObject.transform.position = spawnPos;
        var item = itemObject.GetComponent<Item>();
        item.Init(GetRandomItemType(), new Vector2(xCoordinate, yCoordinate));
        items[xCoordinate, yCoordinate] = item;
    }
    public void ItemDestroyed(Item item)
    {
        Debug.Log("GridManager ItemDestroyed");
        _itemPool.ReturnToPool(item.gameObject);
        CreateNewItem((int)item.coordinate.x, (int)item.coordinate.y);
    }
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        foreach (var item in items)
    //        {
    //            Destroy(item.gameObject);
    //        }
    //        Start();
    //    }
    //}
}
