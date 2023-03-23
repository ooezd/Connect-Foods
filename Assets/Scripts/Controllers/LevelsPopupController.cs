using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopupController : MonoBehaviour
{
    [SerializeField] Transform levelRowContainer;
    [SerializeField] GameObject levelRowPrefab;
    [SerializeField] RectTransform container;
    [SerializeField] VerticalLayoutGroup layoutGroup;

    LevelReader _levelReader;
    List<LevelModel> _levelModels = new();
    List<LevelRow> _levelRows = new();
    Pool _levelRowPool;
    ViewManager _viewManager;
    void Awake()
    {
        _levelReader = LevelReader.Instance;
        _viewManager = ViewManager.Instance;
        _levelModels = _levelReader.GetLevelModels();
        if (_levelRowPool == null)
        {
            _levelRowPool = PoolManager.Instance.GetPool<LevelRow>(levelRowPrefab, 15);
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
        UpdateLayoutSize(_levelModels.Count);
    }
    void UpdateLayoutSize(int rowCount)
    {
        var rowRect = levelRowPrefab.GetComponent<RectTransform>().rect;
        var height = rowRect.height * rowCount + (rowCount - 1) * layoutGroup.spacing;

        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
    void OnLevelRowClick(LevelModel levelModel)
    {
        SessionManager.Instance.SetCurrentLevel(levelModel);
        _viewManager.LoadScene(1);
    }
    public void OnCloseButtonClick()
    {
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        foreach (var row in _levelRows)
        {
            _levelRowPool.ReturnToPool(row.gameObject);
        }
    }
}
