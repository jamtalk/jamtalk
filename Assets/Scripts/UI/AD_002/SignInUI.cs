using System;
using GJGameLibrary;
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
        buttonKakao.onClick.AddListener(() => OnClickSigninSNS(eProvider.kakao));
        buttonNaver.onClick.AddListener(() => OnClickSigninSNS(eProvider.naver));
        buttonFaceBook.onClick.AddListener(() => OnClickSigninSNS(eProvider.facebook));

        eProvider userProvider = eProvider.none;
        if(PlayerPrefs.HasKey("PROVIDER"))
            userProvider = (eProvider)Enum.Parse(typeof(eProvider), PlayerPrefs.GetString("PROVIDER"));
        Debug.Log("signInUI userProvider : " + userProvider.ToString());

        toggleSave.isOn = PlayerPrefs.HasKey("ID");

        if (userProvider == eProvider.none && toggleSave.isOn)
            email.text = PlayerPrefs.GetString("ID");
    }

    private void OnClickSigninSNS(eProvider provider) =>
        GameManager.Instance.SignInSNS(provider, SignIn);

    public void SignIn(string id, string pw, eProvider eProvider, string uid)
    {
        if (string.IsNullOrEmpty(id))
            AndroidPluginManager.Instance.Toast("아이디를 입력하세요.");
        else if (string.IsNullOrEmpty(pw))
            AndroidPluginManager.Instance.Toast("비밀번호를 입력하세요.");
        else
        {
            var param = new SignInParam(id, pw, eProvider, uid);
            RequestManager.Instance.RequestAct(param, (res) =>
            {
                var result = res.GetResult<ActRequestResult>();
                if (result.code != eErrorCode.Success)
                {
                    Debug.Log(result.code + " : " + result.msg);
                    AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
                }
                else
                {

                    if (toggleSave.isOn)
                        PlayerPrefs.SetString("ID", id);
                    else if(PlayerPrefs.HasKey("ID"))
                        PlayerPrefs.DeleteKey("ID");

                    if (toggleAutoSignIn.isOn)
                        PlayerPrefs.SetString("PW", pw);
                    else if (PlayerPrefs.HasKey("PW"))
                        PlayerPrefs.DeleteKey("PW");

                    if (eProvider != eProvider.none)
                    {
                        PlayerPrefs.SetString("ID", id);
                        PlayerPrefs.SetString("UID", uid);
                        PlayerPrefs.SetString("PROVIDER", eProvider.ToString());
                    }
                    else
                    {
                        id = "email:"+email.text;
                        PlayerPrefs.DeleteKey("UID");
                        PlayerPrefs.DeleteKey("PROVIDER");
                    }


                    PlayerPrefs.Save();
                    UserDataManager.Instance.LoadUserData(id, () =>
                    {
                        GJSceneLoader.Instance.LoadScene(eSceneName.AD_003);
                    });
                }
                Debug.Log(res.GetLog());
            });
        }
    }
}   
