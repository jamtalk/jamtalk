using System.Collections;
using System.Collections.Generic;
using Kakaotalk;
using UnityEngine;

public class KakaoSigner : BaseSigner
{
    protected override eProvider provider => eProvider.kakao;
    protected override string snsType => "카카오";
    protected override void SignIn() => GameManager.Instance.StartCoroutine(SigninRoutine());
    private bool isInitalize = false;

    private IEnumerator SigninRoutine()
    {
        var uid = string.Empty;
        var name = string.Empty;
        var email = string.Empty;
        bool isRecived = false;
        if (!isInitalize)
        {
            KakaoSdk.Initialize(() =>
            {
                isInitalize = true;

            }, error =>
            {
                isInitalize = true;
                Debug.Log("initalize error : " + error);
            });
        }

        while (!isInitalize) yield return null;

        KakaoSdk.Login(LoginMethod.Both, (token) =>
        {
            Debug.Log("token :" + JsonUtility.ToJson(token));

            KakaoSdk.GetUserInformation((info) =>
            {
                Debug.Log("info : " + JsonUtility.ToJson(info));
                uid = info.id.ToString();
                email = info.kakao_account.email;

                KakaoSdk.GetProfile((profile) =>
                {
                    Debug.Log("profile : " + JsonUtility.ToJson(profile));
                    name = profile.nickname;

                    Debug.LogFormat("provider : {0}, uid : {1}, name : {2}, email : {3}", "kakao", uid, name, email);
                    isRecived = true;

                }, error => Debug.Log("profileError : " + error));
            }, error => Debug.Log("infoError : " + error));
        }, error => Debug.Log("login : " + error));

        while (!isRecived) { yield return null; }

        ExistSNS(eProvider.kakao, uid, email, name);
    }

    protected override void SignOut()
    {
        KakaoSdk.Logout(() =>
        {
            Debug.Log("kakao logout : + " + UserDataManager.Instance.CurrentUser.user_id);
        }, error => Debug.Log("logoutError : " + error));
    }

}
