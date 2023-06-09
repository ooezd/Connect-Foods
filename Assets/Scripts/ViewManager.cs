using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    [SerializeField] List<GameObject> views;

    public static ViewManager Instance;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    public void LoadPopup(string popupName)
    {
        var view = views.First(v => v.name.Equals(popupName));
        if (view == null)
        {
            Debug.LogError($"Popup name {popupName} couldn't be found.");
        }

        Instantiate(view);
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
