using GJGameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterScene : MonoBehaviour
{
    public Button buttonStart;
    public GameObject loading;
    public AnimationCharactor[] charactors;

    private void Awake()
    {
        buttonStart.onClick.AddListener(() =>
        {
            if (PlayerPrefs.HasKey("PW"))
            {
                loading.gameObject.SetActive(true);
                UserDataManager.Instance.LoadUserData(PlayerPrefs.GetString("ID"),() =>
                {
                    //if(UserDataManager.Instance.UserProvider == eProvider.none)
                    //    SignSNS.Instance.LoginSNS(UserDataManager.Instance.UserProvider);

                    GJSceneLoader.Instance.LoadScene(eSceneName.AD_003);
                });
                
            }
            else
                GJSceneLoader.Instance.LoadScene(eSceneName.AD_002);
        });

        foreach (var item in charactors)
            StartCoroutine(charactorRoutine(item));
    }

    IEnumerator charactorRoutine(AnimationCharactor charactor)
    {
        while (true)
        {
            var randomTime = Random.Range(5, 10);
            yield return new WaitForSecondsRealtime(randomTime);

            charactor.OpningScene();
        }
    }
}
