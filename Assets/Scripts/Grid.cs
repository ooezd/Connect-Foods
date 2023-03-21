using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] SpriteRenderer background;

    [SerializeField] int xDim;
    [SerializeField] int yDim;
    [SerializeField] float referenceSize;
    [SerializeField] float backgroundPreferredSize;

    float offset = .75f;
    Camera _camera;

    Item[,] items;
    Vector2[,] itemPositions;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            Start();
        }
    }
    void Start()
    {
        items = new Item[xDim, yDim];
        itemPositions = new Vector2[xDim, yDim];
        _camera = Camera.main;
        SetCameraPosition();
        //SetScreenBoundaries();
        SetCameraSize();
        CreateGrid();
        Physics2D.queriesHitTriggers = true;
    }
    

    void SetCameraSize()
    {
        var camSize = _camera.orthographicSize;
        var ySize = camSize * 2;
        var xSize = ySize * (float)Screen.width / (float)Screen.height;
        var screenSize = (camSize * xDim / xSize) -(.2f * xDim);
        _camera.orthographicSize = screenSize;
        background.transform.localScale = (screenSize/referenceSize) * backgroundPreferredSize * Vector3.one;

    }
    void CreateGrid()
    {
        for(int i = 0; i < xDim; i++)
        {
            for(int j = 0; j < yDim; j++)
            {
                var coordinate = new Vector2(i, j);
                var spawnPos = coordinate * offset;
                itemPositions[i,j] = spawnPos;
                var tileObject = Instantiate(itemPrefab, spawnPos, Quaternion.identity);//get from pool manager
                var item = tileObject.GetComponent<Item>();
                item.Init(GetRandomItemType(),coordinate);
                items[i, j] = item;
            }
        }
    }
    void SetCameraPosition()
    {
        var xPos = ((xDim - 1) * offset) / 2;
        var yPos = ((yDim - 1) * offset) / 2 + .5f;
        _camera.transform.position = new Vector3(xPos, yPos, -10f);
        background.transform.position = new Vector3(xPos, yPos);
    }
    ItemType GetRandomItemType()
    {
        var index = Random.Range(0, sizeof(ItemType) + 1);
        return (ItemType)index;
    }
}
