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
        buttonKakao.onClick.AddListener(() => StartCoroutine(KakaoLogin()));

        if (!PlayerPrefs.HasKey("SignInSNS"))
        {
            if (PlayerPrefs.HasKey("ID"))
                email.text = PlayerPrefs.GetString("ID");
            toggleSave.isOn = PlayerPrefs.HasKey("ID");
        }
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

    private void ExistSNS(eProvider eProvider ,string uid, string email, string name)
    {
        var providerID = eProvider.ToString().Substring(0, 2) + uid;
        var param = new ExistIDParam(providerID);

        RequestManager.Instance.RequestAct(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();
            if (result.code == eErrorCode.Success)
                SignUpSNS(eProvider, uid, name, email);
            else
                SignIn(providerID, providerID, eProvider, uid);
        });
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

                SignIn(providerID, providerID, eProvider, UID);
            }
        });
    }

    public void SignIn(string id, string pw, eProvider eProvider, string UID)
    {
        var provider = string.Empty;
        if (eProvider == eProvider.none)
        {
            id = email.text;
            pw = this.pw.text;
        }
        else
        {
            provider = eProvider.ToString();
        }

        if (string.IsNullOrEmpty(id))
            AndroidPluginManager.Instance.Toast("아이디를 입력하세요.");
        else if (string.IsNullOrEmpty(pw))
            AndroidPluginManager.Instance.Toast("비밀번호를 입력하세요.");
        else
        {
            var param = new SignInParam(id, pw, provider, UID);
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
