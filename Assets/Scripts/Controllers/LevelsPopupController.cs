using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopupController : MonoBehaviour
{
    [SerializeField] Transform levelRowContainer;
    [SerializeField] GameObject levelRowPrefab;

    LevelReader _levelReader;
    List<LevelModel> _levelModels = new();
    List<LevelRow> _levelRows = new();
    PoolManager _poolManager;
    Pool _levelRowPool;
    ViewManager _viewManager;
    void Awake()
    {
        _levelReader = LevelReader.Instance;
        _poolManager = PoolManager.Instance;
        _viewManager = ViewManager.Instance;
        _levelModels = _levelReader.GetLevelsModel();
        if (_levelRowPool == null)
        {
            _levelRowPool = _poolManager.GetPool<LevelRow>(levelRowPrefab, 10);
        }
        CreateLevelRows();
    }
    void CreateLevelRows()
    {
        for(int i =0; i < _levelModels.Count; i++)
        {
            var go = _levelRowPool.GetFromPool();
            go.transform.SetParent(levelRowContainer);
            var levelRow = go.GetComponent<LevelRow>();
            levelRow.SetData(_levelModels[i],OnLevelRowClick);
            _levelRows.Add(levelRow);
        }
    }
    void OnLevelRowClick(LevelModel levelModel)
    {
        //keep levelModel after scene change
        _viewManager.LoadScene(1);
    }
    public void OnCloseButtonClick()
    {
        foreach(var row in _levelRows)
        {
            _levelRowPool.ReturnToPool(row.gameObject);
        }
        Destroy(gameObject);
    }
}
