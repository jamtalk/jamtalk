using System.Collections;
using System.Collections.Generic;
using GJGameLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            GJSceneLoader.Instance.LoadScene(eSceneName.AD_008);
        });
    }
}
