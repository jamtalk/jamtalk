using UnityEngine;
using UnityEngine.UI;

public class SignUpUI : MonoBehaviour
{
    public Button buttonBack;
    public InputField email;
    public Button buttonCheckExist;
    public InputField pw;
    public InputField pwConfirm;
    public InputField nameField;
    public InputField code;
    public Toggle toggleAll;
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
        toggleAll.onValueChanged.AddListener(value =>
        {
            if (value)
            {
                toggleAge.isOn = true;
                toggleServiceTerms.isOn = true;
                togglepolicy.isOn = true;
                toggleTerms.isOn = true;
                toggleReciveEvent.isOn = true;
            }
        });
        email.onValueChanged.AddListener((value) => isChecked = false);
        buttonCheckExist.onClick.AddListener(CheckExist);
        toggleAge.onValueChanged.AddListener(AgreeToggleValueChanged);
        toggleServiceTerms.onValueChanged.AddListener(AgreeToggleValueChanged);
        togglepolicy.onValueChanged.AddListener(AgreeToggleValueChanged);
        toggleTerms.onValueChanged.AddListener(AgreeToggleValueChanged);
        toggleReciveEvent.onValueChanged.AddListener(AgreeToggleValueChanged);
        buttonConfirm.onClick.AddListener(RegistID);
        buttonBack.onClick.AddListener(Clear);
    }
    private void AgreeToggleValueChanged(bool value)
    {
        if (!value)
            toggleAll.isOn = false;
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
            var param = new SignUpParam(email.text, pw.text, nameField.text, email.text, code.text, eProvider.none, string.Empty, string.Empty, string.Empty, string.Empty);
            Debug.LogFormat("{0}/{1}", email.text, param.user_id);
            RequestManager.Instance.RequestAct(param, callback =>
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
            var param = new ExistIDParam(email.text);

            RequestManager.Instance.RequestAct(param, callback =>
            {
                loading.gameObject.SetActive(false);
                isChecked = callback.GetResult<ActRequestResult>().code == eErrorCode.Success;
                if (isChecked)
                    AndroidPluginManager.Instance.Toast("사용 가능한 계정 입니다");
                else
                    AndroidPluginManager.Instance.Toast("이미 존재하는 계정 입니다");
            });
        }
    }

    private void Clear()
    {
        email.text = string.Empty;
        pw.text = string.Empty;
        pwConfirm.text = string.Empty;
        nameField.text = string.Empty;
        code.text = string.Empty;
        toggleAll.isOn = false;
        toggleAge.isOn = false;
        toggleServiceTerms.isOn = false;
        togglepolicy.isOn = false;
        toggleReciveEvent.isOn = false;
        toggleReciveEvent.isOn = false;
        buttonConfirm.interactable = false;
        isChecked = false;
        gameObject.SetActive(false);
    }
}
