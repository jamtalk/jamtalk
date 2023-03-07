using System.Collections;
using System.Collections.Generic;
using GJGameLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button button;
    
    public eSceneName exitScene = eSceneName.AD_008;
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            GJSceneLoader.Instance.LoadScene(exitScene);
        });
    }
}
