using System;
using System.Linq;
using GJGameLibrary;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject loading;

    public FindAccount findAccountOrizin;
    private FindAccount findAccount;
    private void Awake()
    {
        buttonRegist.onClick.AddListener(() =>
        {
            email.text = string.Empty;
            pw.text = string.Empty;
            signUp.gameObject.SetActive(true);
        });

        buttonSignIn.onClick.AddListener(() =>
        {
            GameManager.Instance.SignIn(email.text, this.pw.text, eProvider.none, string.Empty,() =>
            {
                SetPlayerprefs(email.text, pw.text, eProvider.none, string.Empty);
            });
        });
        buttonKakao.onClick.AddListener(() => OnClickSigninSNS(eProvider.kakao));
        buttonNaver.onClick.AddListener(() => OnClickSigninSNS(eProvider.naver));
        buttonFaceBook.onClick.AddListener(() => OnClickSigninSNS(eProvider.facebook));
        buttonFindPW.onClick.AddListener(FindPW);

        eProvider userProvider = eProvider.none;
        if(PlayerPrefs.HasKey("PROVIDER"))
            userProvider = (eProvider)Enum.Parse(typeof(eProvider), PlayerPrefs.GetString("PROVIDER"));

        toggleSave.isOn = PlayerPrefs.HasKey("ID");

        if (userProvider == eProvider.none && toggleSave.isOn)
            email.text = PlayerPrefs.GetString("ID");

        var url = GameManager.Instance.schema.GetYoutubeURL("jamjam");
        Debug.LogFormat("@@@@@URL : {0}", url[0].songURL);
    }

    private void OnClickSigninSNS(eProvider provider)
    {
        loading.transform.SetAsLastSibling();
        loading.SetActive(true);
        GameManager.Instance.SignInSNS(provider, SetPlayerprefs);
    }

    private void FindPW()
    {
        if (findAccount == null)
        {
            findAccount = Instantiate(findAccountOrizin, transform.parent);
        }
        else
            findAccount.gameObject.SetActive(true);

        findAccount.Init(FindAccount.ePanelType.FindPW);
    }

    private void SetPlayerprefs(string id, string pw, eProvider provider, string uid)
    {
        if(provider == eProvider.none)
        {
            id = email.text;
            pw = this.pw.text;
            uid = string.Empty;
        }
        
        if (toggleSave.isOn)
            PlayerPrefs.SetString("ID", id);
        else if (PlayerPrefs.HasKey("ID"))
            PlayerPrefs.DeleteKey("ID");

        if (toggleAutoSignIn.isOn)
            PlayerPrefs.SetString("PW", pw);
        else if (PlayerPrefs.HasKey("PW"))
            PlayerPrefs.DeleteKey("PW");

        if (provider != eProvider.none)
        {
            PlayerPrefs.SetString("ID", id);
            PlayerPrefs.SetString("UID", uid);
            PlayerPrefs.SetString("PROVIDER", provider.ToString());
        }
        else
        {
            id = "email:" + email.text;
            PlayerPrefs.DeleteKey("UID");
            PlayerPrefs.DeleteKey("PROVIDER");
        }

        PlayerPrefs.Save();
        GameManager.Instance.SignIn(id, pw, provider, uid);
    }
}   
