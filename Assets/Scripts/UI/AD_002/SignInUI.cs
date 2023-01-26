using GJGameLibrary;
using Kakaotalk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignInUI : MonoBehaviour
{
    public InputField email;
    public InputField pw;
    public Toggle toggleSave;
    public Toggle toggleAutoSignIn;
    public Button buttonFindPW;
    public Button buttonRegist;
    public Button buttonFaceBook;
    public Button buttonKakao;
    public Button buttonNaver;
    public Button buttonSignIn;
    public GameObject loading;
    public GameObject signUp;
    private void Awake()
    {
        buttonRegist.onClick.AddListener(() =>
        {
            email.text = string.Empty;
            pw.text = string.Empty;
            signUp.gameObject.SetActive(true);
        });
        buttonSignIn.onClick.AddListener(() => SignIn(email.text, pw.text, eProvider.none, string.Empty));
        buttonKakao.onClick.AddListener(KakaoLogin);


        if (PlayerPrefs.HasKey("ID"))
            email.text = PlayerPrefs.GetString("ID");

        toggleSave.isOn = PlayerPrefs.HasKey("ID");
    }
    private void KakaoLogin()
    {
        String keyHash = KakaoSdk.GetKeyHash();
        Debug.Log("key hash : " +  keyHash);

        KakaoSdk.Initialize(() => {
            KakaoSdk.Login(LoginMethod.Both, (token) => {
                Debug.Log("token :" + JsonUtility.ToJson(token));

                KakaoSdk.GetUserInformation((info) =>
                {
                    Debug.Log("info : " + JsonUtility.ToJson(info));
                }, e => Debug.Log("infoError : e"));

                KakaoSdk.GetProfile((profile) => {
                    Debug.Log("profile : " + JsonUtility.ToJson(profile));
                }, e => Debug.Log("profileError : " + e));
            }, e => Debug.Log("login : " + e));
        }, e => Debug.Log("iniitalizeError : " + e));
    }

    private void SignUpSNS(eProvider eProvider, string UID, string name, string email)
    {
        var providerID = eProvider.ToString().Substring(0,2) + UID;
        var param = new SignUpParam(providerID, providerID, name, email, string.Empty, string.Empty, eProvider.ToString(), string.Empty, UID, string.Empty);

        RequestManager.Instance.RequestAct(param, res =>
        {
            var result = res.GetResult<ActRequestResult>();

            if(result.code != eErrorCode.Success)
            {
                //fail
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
                
                AndroidPluginManager.Instance.Toast(string.Format("{0} 회원가입이 완료되었습니다", snsType));

                SignIn(providerID, providerID, eProvider, UID);
            }
        });
    }

    public void SignIn(string id, string pw, eProvider eProvider, string UID)
    {
        if (string.IsNullOrEmpty(id))
            id = email.text;
        if (string.IsNullOrEmpty(pw))
            pw = this.pw.text;

        if (string.IsNullOrEmpty(id))
            AndroidPluginManager.Instance.Toast("아이디를 입력하세요.");
        else if (string.IsNullOrEmpty(pw))
            AndroidPluginManager.Instance.Toast("비밀번호를 입력하세요.");
        else
        {
            var provider = string.Empty;
            if (eProvider != eProvider.none)
                provider = eProvider.ToString();
            else
                id = "email:" + id;

            var param = new SignInParam(id, pw, provider, UID);
            loading.gameObject.SetActive(true);
            RequestManager.Instance.RequestAct(param, (res) =>
            {
                var result = res.GetResult<ActRequestResult>();
                if (result.code != eErrorCode.Success)
                {
                    loading.gameObject.SetActive(false);
                    Debug.Log(result.code);
                    AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
                }
                else
                {
                    if(toggleSave.isOn)
                        PlayerPrefs.SetString("ID", id);
                    else if(PlayerPrefs.HasKey("ID"))
                        PlayerPrefs.DeleteKey("ID");

                    if (toggleAutoSignIn.isOn)
                        PlayerPrefs.SetString("PW", pw);
                    else if (PlayerPrefs.HasKey("PW"))
                        PlayerPrefs.DeleteKey("PW");

                    PlayerPrefs.Save();
                    UserDataManager.Instance.LoadUserData(email.text, () =>
                    {
                        GJSceneLoader.Instance.LoadScene(eSceneName.AD_003);
                    });
                }
                Debug.Log(res.GetLog());
            });
        }
    }
}   
