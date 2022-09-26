using GJGameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterScene : MonoBehaviour
{
    public Button buttonStart;
    public GameObject loading;
    private void Awake()
    {
        buttonStart.onClick.AddListener(() =>
        {
            if (PlayerPrefs.HasKey("PW"))
            {
                loading.gameObject.SetActive(true);
                UserDataManager.Instance.LoadUserData(PlayerPrefs.GetString("ID"),() =>
                {
                    GJSceneLoader.Instance.LoadScene(eSceneName.AD_003);
                });
                
            }
            else
                GJSceneLoader.Instance.LoadScene(eSceneName.AD_002);
        });
    }
}
