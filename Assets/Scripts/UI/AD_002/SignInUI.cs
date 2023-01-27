using System;
using System.Collections;
using GJGameLibrary;
using Kakaotalk;
using Newtonsoft.Json.Linq;
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
        buttonKakao.onClick.AddListener(() => SignSNS.Instance.LoginSNS(eProvider.kakao));
        buttonNaver.onClick.AddListener(() => SignSNS.Instance.LoginSNS(eProvider.naver));
        buttonFaceBook.onClick.AddListener(() => SignSNS.Instance.LoginSNS(eProvider.facebook));

        if (!PlayerPrefs.HasKey("SignInSNS"))
        {
            if (PlayerPrefs.HasKey("ID"))
                email.text = PlayerPrefs.GetString("ID");
            toggleSave.isOn = PlayerPrefs.HasKey("ID");
        }

        SignSNS.Instance.onSignIn += SignIn;
    }

    public void SignIn(string id, string pw, eProvider eProvider, string UID)
    {
        if (string.IsNullOrEmpty(id))
            AndroidPluginManager.Instance.Toast("아이디를 입력하세요.");
        else if (string.IsNullOrEmpty(pw))
            AndroidPluginManager.Instance.Toast("비밀번호를 입력하세요.");
        else
        {
            var param = new SignInParam(id, pw, eProvider, UID);
            loading.gameObject.SetActive(true);
            RequestManager.Instance.RequestAct(param, (res) =>
            {
                var result = res.GetResult<ActRequestResult>();
                if (result.code != eErrorCode.Success)
                {
                    loading.gameObject.SetActive(false);
                    Debug.Log(result.code + " : " + result.msg);
                    AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
                }
                else
                {
                    if(toggleSave.isOn && eProvider == eProvider.none)
                        PlayerPrefs.SetString("ID", id);
                    else if(PlayerPrefs.HasKey("ID"))
                        PlayerPrefs.DeleteKey("ID");

                    if (toggleAutoSignIn.isOn)
                    {
                        if (eProvider != eProvider.none)
                        {
                            PlayerPrefs.SetString("ID", id);
                            PlayerPrefs.SetInt("SignInSNS", System.Convert.ToInt16(true));
                        }
                        else
                            PlayerPrefs.DeleteKey("SignInSNS");

                        PlayerPrefs.SetString("PW", pw);
                    }
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
