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

                var id = PlayerPrefs.GetString("ID");
                var pw = PlayerPrefs.GetString("PW");
                var uid = string.Empty;
                var provider = eProvider.none;

                if (PlayerPrefs.HasKey("UID"))
                    uid = PlayerPrefs.GetString("UID");

                if (PlayerPrefs.HasKey("PROVIDER"))
                    provider = (eProvider)Enum.Parse(typeof(eProvider), PlayerPrefs.GetString("PROVIDER"));
                else
                    id = "email:" + id;

                Debug.LogFormat("id : {0}, pw : {1} uid : {2}, provider : {3}", id, pw, uid, provider);

                UserDataManager.Instance.LoadUserData(id,() =>
                {
                    //SignInUI.SignIn(id, pw, provider, uid);
                    GameManager.Instance.SignIn(id, pw, provider, uid);
                    
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
