using System;
using System.Collections;
using System.Collections.Generic;
using GJGameLibrary;
using System.Runtime.CompilerServices;
using Kakaotalk;
using UnityEngine;
using GJGameLibrary.DesignPattern;

public class SignSNS : MonoSingleton<SignSNS>
{
    public Action<string , string , eProvider , string > onSignIn;

    public void LoginSNS(eProvider provider)
    {
        if (provider == eProvider.kakao)
            StartCoroutine(KakaoLogin());
        else if (provider == eProvider.naver)
            StartCoroutine(NaverLogin());
        else if (provider == eProvider.facebook)
            StartCoroutine(FaceBookLogin());
    }

    private IEnumerator NaverLogin()
    {
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator FaceBookLogin()
    {
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator KakaoLogin()
    {
        var uid = string.Empty;
        var name = string.Empty;
        var email = string.Empty;
        bool isRecived = false;
        KakaoSdk.Initialize(() => {

            KakaoSdk.Login(LoginMethod.Both, (token) => {
                Debug.Log("token :" + JsonUtility.ToJson(token));

                KakaoSdk.GetUserInformation((info) =>
                {
                    Debug.Log("info : " + JsonUtility.ToJson(info));
                    uid = info.id.ToString();
                    email = info.kakao_account.email;

                    KakaoSdk.GetProfile((profile) => {
                        Debug.Log("profile : " + JsonUtility.ToJson(profile));
                        name = profile.nickname;

                        Debug.LogFormat("provider : {0}, uid : {1}, name : {2}, email : {3}", "kakao", uid, name, email);
                        isRecived = true;

                    }, error => Debug.Log("profileError : " + error));
                }, error => Debug.Log("infoError : " + error));
            }, error => Debug.Log("login : " + error));
        }, error => Debug.Log("iniitalizeError : " + error));

        while (!isRecived) { yield return null; }

        ExistSNS(eProvider.kakao, uid, email, name);
    }

    public void ExistSNS(eProvider eProvider, string uid, string email, string name)
    {
        var providerID = eProvider.ToString().Substring(0, 2) + uid;
        var param = new ExistIDParam(providerID);

        RequestManager.Instance.RequestAct(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();
            if (result.code == eErrorCode.Success)
                SignUpSNS(eProvider, uid, name, email);
            else
                onSignIn?.Invoke(providerID, providerID, eProvider, uid);
        });
    }

    public void SignUpSNS(eProvider eProvider, string uid, string name, string email)
    {
        var providerID = eProvider.ToString().Substring(0, 2) + uid;
        var param = new SignUpParam(providerID, providerID, name, email, string.Empty, eProvider, string.Empty, uid, string.Empty, string.Empty);
        RequestManager.Instance.RequestAct(param, res =>
        {
            var result = res.GetResult<ActRequestResult>();

            if (result.code != eErrorCode.Success)
            {
                AndroidPluginManager.Instance.Toast(result.msg);
                Debug.Log("SignUp SNS Failed : " + result.msg);
            }
            else
            {
                string snsType = string.Empty;
                if (eProvider == eProvider.kakao)
                    snsType = "카카오";
                else if (eProvider == eProvider.naver)
                    snsType = "네이버";
                else
                    snsType = "페이스북";

                Debug.Log(string.Format("{0} 회원가입이 완료되었습니다", snsType));
                AndroidPluginManager.Instance.Toast(string.Format("{0} 회원가입이 완료되었습니다", snsType));

                onSignIn?.Invoke(providerID, providerID, eProvider, uid);
            }
        });
    }

    public void SignOutSNS()
    {
        var temp = string.IsNullOrEmpty(UserDataManager.Instance.CurrentUser.provider);
        var provider = temp ? eProvider.none : ((eProvider)Enum.Parse(typeof(eProvider), UserDataManager.Instance.CurrentUser.provider));
        Debug.Log("provider : " + provider.ToString());
        if (provider == eProvider.kakao)
        {
            KakaoSdk.Logout(() =>
            {
                Debug.Log("kakao logout : + " + UserDataManager.Instance.CurrentUser.user_id);
            }, error => Debug.Log("logoutError : " + error));
        }
        else if (provider == eProvider.naver)
        {

        }
        else if (provider == eProvider.facebook)
        {

        }
        else
        {

        }
    }
}
