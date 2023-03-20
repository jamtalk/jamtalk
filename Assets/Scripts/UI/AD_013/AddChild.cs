using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddChild : MonoBehaviour
{
    public TMP_InputField inputName;
    public TMP_InputField inputBirth;
    public Toggle maleToggle;
    public Toggle femaleToggle;
    public Toggle termsToggle;
    public Button termsMoreButton;
    public Button confirmButton;
    public GameObject terms;
    public Action onAdd;

    private void Awake()
    {
        termsMoreButton.onClick.AddListener(() => Application.OpenURL("https://jamtalk.live/privacy"));
        confirmButton.onClick.AddListener(InvalidInput);
    }

    private void InvalidInput()
    {
        if (string.IsNullOrEmpty(inputName.text))
            AndroidPluginManager.Instance.Toast("아이 이름을 입력하세요.");
        else if (string.IsNullOrEmpty(inputBirth.text))
            AndroidPluginManager.Instance.Toast("아이 생년월일을 입력하세요.");
        else if (inputBirth.text.Length<8)
            AndroidPluginManager.Instance.Toast("아이 생년월일을 정확히 입력하세요.");
        else if (!maleToggle.isOn && !femaleToggle.isOn)
            AndroidPluginManager.Instance.Toast("아이 성별을 선택하세요.");
        else if (!termsToggle.isOn)
            AndroidPluginManager.Instance.Toast("아이 정보 수집 약관에 동의하세요.");
        else
            ConfirmAction();
    }

    private void ConfirmAction()
    {
        var gender = maleToggle.isOn ? 'M' : 'F';
        var param = new ChildParam(inputName.text, inputBirth.text, true, 1, 1, gender);

        RequestManager.Instance.Request(param, res =>
        {
            var result = res.GetResult<ActRequestResult>();

            if(result.code == eErrorCode.Success)
            {
                AndroidPluginManager.Instance.Toast("아이가 추가되었습니다.");
                onAdd?.Invoke();
            }
        });
    }

    public void ExitAction()
    {
        inputName.text = string.Empty;
        inputBirth.text = string.Empty;
        termsToggle.isOn = false;
        maleToggle.isOn = false;
        femaleToggle.isOn = false;
        gameObject.SetActive(false);
    }

}
