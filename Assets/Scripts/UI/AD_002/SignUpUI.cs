using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SignUpUI : MonoBehaviour
{
    public Button buttonBack;
    public Button buttonCheckExist;
    public TMP_InputField email;
    public TMP_InputField pw;
    public TMP_InputField pwConfirm;
    public TMP_InputField nameField;
    public TMP_InputField birth;
    public TMP_InputField code;

    //public Toggle toggleAll;
    public Button toggleAll;
    public Image checkAll;
    public Toggle toggleAge;
    public Toggle toggleServiceTerms;
    public Toggle togglepolicy;
    public Toggle toggleTerms;
    public Toggle toggleReciveEvent;
    public Button buttonConfirm;
    public GameObject loading;

    public bool isChecked = false;
    private void Awake()
    {
        toggleAll.onClick.AddListener(() =>
        {
            var isAll = !checkAll.gameObject.activeSelf;
            checkAll.gameObject.SetActive(isAll);

            toggleAge.isOn = isAll;
            toggleServiceTerms.isOn = isAll;
            togglepolicy.isOn = isAll;
            toggleTerms.isOn = isAll;
            toggleReciveEvent.isOn = isAll;
        });

        email.onValueChanged.AddListener((value) => isChecked = false);

        toggleAge.onValueChanged.AddListener(AgreeToggleValueChanged);
        toggleServiceTerms.onValueChanged.AddListener(AgreeToggleValueChanged);
        togglepolicy.onValueChanged.AddListener(AgreeToggleValueChanged);
        toggleTerms.onValueChanged.AddListener(AgreeToggleValueChanged);
        toggleReciveEvent.onValueChanged.AddListener(AgreeToggleValueChanged);

        buttonCheckExist.onClick.AddListener(CheckExist);
        buttonConfirm.onClick.AddListener(RegistID);
        buttonBack.onClick.AddListener(Clear);
    }

    private void AgreeToggleValueChanged(bool value)
    {
        if (!value)
            checkAll.gameObject.SetActive(false);

        if (CheckToggle() && toggleReciveEvent.isOn)
            checkAll.gameObject.SetActive(true);
    }
    private bool CheckToggle()
    {
        return toggleAge.isOn &&
            toggleServiceTerms.isOn &&
            togglepolicy.isOn &&
            toggleTerms.isOn;
    }
    private void RegistID()
    {
        if (string.IsNullOrEmpty(email.text) || !email.text.Contains("@") || !email.text.Contains(".") || email.text.Contains(" "))
        {
            AndroidPluginManager.Instance.Toast("올바른 이메일을 입력하세요");
        }
        else if (string.IsNullOrEmpty(pw.text) || pw.text.Length<6)
        {
            AndroidPluginManager.Instance.Toast("비밀번호를 6자리 이상 입력하세요");
        }
        else if(pwConfirm.text != pw.text)
        {
            AndroidPluginManager.Instance.Toast("비밀번호가 일치하지 않습니다");
        }
        else if (string.IsNullOrEmpty(nameField.text))
        {
            AndroidPluginManager.Instance.Toast("이름을 입력하세요");
        }
        else if (!CheckToggle())
        {
            AndroidPluginManager.Instance.Toast("약관에 동의해주세요");
        }
        else
        {
            loading.gameObject.SetActive(true);
            // param 에 birth 추가 필요 
            var param = new SignUpParam(email.text, pw.text, nameField.text, email.text, code.text, eProvider.none, string.Empty, string.Empty, string.Empty, string.Empty);
            Debug.LogFormat("{0}/{1}", email.text, param.user_id);
            RequestManager.Instance.Request(param, callback =>
            {
                loading.gameObject.SetActive(false);
                var res = callback.GetResult<ActRequestResult>();
                if (res.code == eErrorCode.Success)
                {
                    AndroidPluginManager.Instance.Toast("회원가입이 완료되었습니다");
                    Clear();
                }
                else
                {
                    AndroidPluginManager.Instance.Toast(res.msg);
                }
                Debug.Log(callback.GetLog());
            });
        }
    }
    private void CheckExist()
    {
        if (string.IsNullOrEmpty(email.text) || !email.text.Contains("@") || !email.text.Contains(".") || email.text.Contains(" "))
        {
            AndroidPluginManager.Instance.Toast("올바른 이메일을 입력하세요");
        }
        else
        {
            loading.gameObject.SetActive(true);
            var param = new Exists_emailParam(email.text);

            RequestManager.Instance.Request(param, callback =>
            {
                loading.gameObject.SetActive(false);
                var result = callback.GetResult<ActRequestResult>();
                isChecked = callback.GetResult<ActRequestResult>().code == eErrorCode.Success;

                if (result.code == eErrorCode.Success)
                    AndroidPluginManager.Instance.Toast("사용 가능한 계정 입니다");
                else
                    AndroidPluginManager.Instance.Toast(result.msg);
            });
        }
    }

    private void Clear()
    {
        email.text = string.Empty;
        pw.text = string.Empty;
        pwConfirm.text = string.Empty;
        nameField.text = string.Empty;
        birth.text = string.Empty;
        code.text = string.Empty;

        //toggleAll.isOn = false;

        //buttonConfirm.interactable = false;
        isChecked = false;
        gameObject.SetActive(false);
    }
}
