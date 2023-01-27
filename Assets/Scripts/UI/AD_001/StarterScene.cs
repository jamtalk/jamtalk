using System;
using System.Collections;
using GJGameLibrary;
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
                    Debug.Log(UserDataManager.Instance.UserProvider);
                    var provider = UserDataManager.Instance.UserProvider;
                    GameManager.Instance.SignInSNS(provider, new SignInUI().SignIn);

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
            var randomTime = UnityEngine.Random.Range(5, 10);
            yield return new WaitForSecondsRealtime(randomTime);

            charactor.OpningScene();
        }
    }
}
