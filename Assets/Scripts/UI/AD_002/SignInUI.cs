using GJGameLibrary;
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
        buttonSignIn.onClick.AddListener(SignIn);

        if (PlayerPrefs.HasKey("ID"))
            email.text = PlayerPrefs.GetString("ID");

        toggleSave.isOn = PlayerPrefs.HasKey("ID");
    }
    public void SignIn()
    {
        if (string.IsNullOrEmpty(email.text))
        {
            AndroidPluginManager.Instance.Toast("이메일을 입력하세요");
        }
        else if (string.IsNullOrEmpty(pw.text))
        {
            AndroidPluginManager.Instance.Toast("비밀번호를 입력하세요");
        }
        else
        {
            Debug.Log("리퀘스트 보냄");
            var param = new SignInParam("email:"+email.text, pw.text, string.Empty, string.Empty);
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
                        PlayerPrefs.SetString("ID", email.text);
                    else if(PlayerPrefs.HasKey("ID"))
                        PlayerPrefs.DeleteKey("ID");

                    if (toggleAutoSignIn.isOn)
                        PlayerPrefs.SetString("PW", pw.text);
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
