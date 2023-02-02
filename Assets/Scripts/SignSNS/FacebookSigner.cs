using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FacebookSigner : BaseSigner
{
    protected override eProvider provider => eProvider.facebook;

    protected override string snsType => "페이스북";

    protected override void SignIn()
    {
        GameManager.Instance.StartCoroutine(SigninFB());
    }

    protected override void SignOut()
    {
        FB.LogOut();
    }
    private IEnumerator SigninFB()
    {
        Debug.Log("페이스북 로그인 프로세스 시작");
        if (!FB.IsInitialized)
        {
            Debug.Log("페이스북 이니셜라이징");
            FB.Init(InitCallback,OnHideUnity);
        }

        while (!FB.IsInitialized) { yield return null; }
        Debug.Log("로그아웃 진행");
        FB.LogOut();
        Debug.Log("로그아웃 완료");
        bool success=false;
        bool recived = false;
        string uid = string.Empty;
        string email = string.Empty;
        string name = string.Empty;
        Debug.Log("페이스북 로그인 요청");
        FB.LogInWithReadPermissions(new string[] {
            "email",
            "public_profile",
            "user_friends"
        }, (result) =>
        {
            Debug.LogFormat("로그인 요청 결과\n", result);
            success = FB.IsLoggedIn;
            Debug.LogFormat("로그인 성공여부 : {0}", FB.IsLoggedIn);
            if (FB.IsLoggedIn)
            {
                var token = AccessToken.CurrentAccessToken;
                uid = token.UserId;
                Debug.Log("받아온 UID: " + token.UserId);
                Debug.Log("받아온 Access Token: " + token.TokenString);
                Debug.LogFormat("토큰의 권한 목록 : {0}", string.Join(", ", token.Permissions));
                Debug.LogFormat("받아온 토큰 상태\n{0}", JObject.Parse(token.ToJson()));
                FB.API("/me?fields=name,picture", HttpMethod.GET, (callback)=>
                {
                    Debug.Log("이름 요청 완료");
                    Debug.Log(JObject.Parse(callback.RawResult));
                    var dic = callback.ResultDictionary;
                    name = dic["name"].ToString();
                    if (dic.ContainsKey("email"))
                        email = dic["email"].ToString();
                    else
                        email = string.Empty;
                    recived = true;
                });
            }
            else
            {
                Debug.Log("User cancelled login");
                recived = true;
            }
        });

        while (!recived) { yield return null; }

        if (success)
            ExistSNS(eProvider.facebook, uid, email, name);
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    /// <summary>
    /// 로그인했던 이력이 있으면 재로그인
    /// </summary>
    //private void ReSignwithFacebook()
    //{
    //    FB.Mobile.RefreshCurrentAccessToken(RefreshCallback);
    //}

    //private void RefreshCallback(IAccessTokenRefreshResult result)
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        var token = result.AccessToken;

    //        Debug.Log("UID: " + token.UserId);
    //        Debug.Log("Access Token: " + token.TokenString);
    //    }
    //    else
    //    {
    //        Debug.Log("Error: " + result.Error);
    //    }
    //}

    private void OnHideUnity(bool isGameShown)
    {
        //facebook 로그인 과정에서 시간을 멈추는 역할
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
