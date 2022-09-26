using GJGameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneButton : MonoBehaviour
{
    public eSceneName scene;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(LoadScene);
    }
    public void LoadScene()
    {
        GJSceneLoader.Instance.LoadScene(scene);
    }
}
