using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
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
        if (!FB.IsInitialized)
            FB.Init(InitCallback,OnHideUnity);

        while (!FB.IsInitialized) { yield return null; }
        bool success=false;
        bool recived = false;
        AccessToken token = null;
        Profile profile = null;
        FB.LogInWithReadPermissions(new string[] { "name", "email", "id" }, (result)=>
        {
            success = FB.IsLoggedIn;
            if (FB.IsLoggedIn)
            {
                token = AccessToken.CurrentAccessToken;
                Debug.Log("UID: " + token.UserId);
                Debug.Log("Access Token: " + token.TokenString);
                Debug.LogFormat("받아온 토큰 상태\n{0}", token.ToJson());
                foreach (string perm in token.Permissions)
                {
                    // 접근 권한이 있는 항목들
                    Debug.Log("perm: " + perm);
                }
                FB.CurrentProfile((profile_result)=>
                {
                    Debug.LogFormat("profile 결과\n{0}", profile_result.RawResult);
                    profile = profile_result.CurrentProfile;
                });
            }
            else
            {
                Debug.Log("User cancelled login");
            }
            recived = true;
        });

        while (!recived) { yield return null; }

        ExistSNS(eProvider.facebook, token.UserId, profile.Email, profile.Name);
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

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var token = AccessToken.CurrentAccessToken;

            Debug.Log("UID: " + token.UserId);
            Debug.Log("Access Token: " + token.TokenString);
            Debug.LogFormat("받아온 토큰 상태\n{0}", token.ToJson());
            foreach (string perm in token.Permissions)
            {
                // 접근 권한이 있는 항목들
                Debug.Log("perm: " + perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
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
