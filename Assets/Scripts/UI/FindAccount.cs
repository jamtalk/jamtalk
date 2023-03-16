using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindAccount : MonoBehaviour
{
    public enum ePanelType
    {
        FindID,
        FindPW,
        ChangePW,
    }

    public ePanelType panelType = ePanelType.FindID;
    public Button exitButton;
    public Text title;
    public Text detail;
    public TMP_InputField firstInput;
    public TMP_InputField secondInput;
    public Text firstTitle;
    public Text secondTitle;
    public Button confirmButton;

    public GameObject reactionPopup;
    private Button exitPopupButton => reactionPopup.GetComponentInChildren<Button>();
    private Text exitPopupText => reactionPopup.GetComponentInChildren<Text>();
    private UserInfoData user;
    private bool isValid = false;
    public GameObject loading;

    private void Awake()
    {
        user = UserDataManager.Instance.CurrentUser;

        exitButton.onClick.AddListener(() => gameObject.SetActive(false));
        confirmButton.onClick.AddListener(ConfirmAction);
        exitPopupButton.onClick.AddListener(() =>
        {
            reactionPopup.gameObject.SetActive(false);
            if (isValid)
                gameObject.SetActive(false);
        });
    }

    private void ConfirmAction()
    {
        if (firstInput.text == string.Empty) return;
        if (secondInput.text == string.Empty) return;
        loading.transform.SetAsLastSibling();
        loading.SetActive(true);
        CheckVolid(SetReactionPopup);
    }


    private void SetReactionPopup(bool isValid)
    {
        loading.SetActive(false);
        this.isValid = isValid;
        switch (panelType)
        {
            case ePanelType.FindID:
                if (isValid)
                {
                    var id = user.user_id.Replace("email:", string.Empty);
                    ShowMessage(string.Format("회원님의 아이디는 \"{0}\" 입니다.", id));
                }
                else
                {
                    ShowMessage("입력한 정보와 일치하는 아이디가 없습니다.");
                }
                break;
            case ePanelType.FindPW:
                if (isValid)
                {
                    Init(ePanelType.ChangePW);
                }
                else
                {
                    ShowMessage("입력한 정보와 일치하는 아이디가 없습니다.");
                }
                break;
            case ePanelType.ChangePW:
                if (isValid)
                {
                    var param = new MemberInfoParam(user, MemberInfoParam.eMemberInfo.user_pw, firstInput.text);

                    RequestManager.Instance.Request(param, (res) =>
                    {
                        var result = res.GetResult<ActRequestResult>();

                        if (result.code != eErrorCode.Success)
                        {
                            Debug.Log(result.msg);
                        }
                        else
                        {
                            ShowMessage("입력하신 비밀번호로 변경되었습니다.");
                        }
                    });
                }
                else
                {
                    ShowMessage("입력하신 비밀번호가 일치하지 않습니다.");
                }
                break;
        }
    }

    private void ShowMessage(string message)
    {
        Debug.LogFormat("메세지 출력 : {0}", message);
        exitPopupText.text = message;
        reactionPopup.gameObject.SetActive(true);
    }

    private void CheckVolid(Action<bool> callback)
    {
        switch (panelType)
        {
            case ePanelType.FindID:
                callback?.Invoke(firstInput.text == user.name && secondInput.text == user.birth);
                break;
            case ePanelType.FindPW:
                var param = new UserInfoParam(string.Format("email:{0}", firstInput.text));
                RequestManager.Instance.Request(param, (response) =>
                {
                    var result = response.GetResult<DataRequestResult<UserInfoData>>();
                    if (result.code == eErrorCode.Success && result.data.name == secondInput.text)
                    {
                        this.user = result.data;
                        callback?.Invoke(true);
                    }
                    else
                    {
                        callback?.Invoke(false);
                    }
                });
                break;
            case ePanelType.ChangePW:
                callback?.Invoke(firstInput.text == secondInput.text);
                break;
        }
    }

    public void Init(ePanelType target)
    {
        this.panelType = target;
        gameObject.SetActive(true);

        firstInput.text = string.Empty;
        secondInput.text = string.Empty;

        if(target == ePanelType.FindPW)
        {
            title.text = "비밀번호 찾기";
            detail.text = "비밀번호를 찾을 아이디의 정보를 입력해주세요.";
            firstTitle.text = "이메일";
            firstInput.placeholder.GetComponent<TextMeshProUGUI>().text = "이메일을 입력해 주세요";
            firstInput.contentType = TMP_InputField.ContentType.EmailAddress;
            secondTitle.text = "실명";
            secondInput.placeholder.GetComponent<TextMeshProUGUI>().text = "실명을 입력해 주세요";
            secondInput.contentType = TMP_InputField.ContentType.Name;
        }
        else if( target == ePanelType.ChangePW)
        {
            title.text = "비밀번호 변경";
            detail.text = "새로운 비밀번호를 입력하해 주세요";
            firstTitle.text = "변경할 비밀번호";
            firstInput.placeholder.GetComponent<TextMeshProUGUI>().text = "비밀번호를 입력해 주세요";
            firstInput.contentType = TMP_InputField.ContentType.Password;
            secondTitle.text = "변경할 비밀번호 확인";
            secondInput.placeholder.GetComponent<TextMeshProUGUI>().text = "비밀번호를 한번 더 입력해 주세요";
            secondInput.contentType = TMP_InputField.ContentType.Password;
        }
        else if (target == ePanelType.FindID)
        {
            title.text = "아이디 찾기";
            detail.text = "아이디를 찾을 회원님의 정보를 입력해주세요.";
            firstTitle.text = "실명";
            firstInput.placeholder.GetComponent<TextMeshProUGUI>().text = "실명을 입력해 주세요";
            secondTitle.text = "생년월일";
            secondInput.placeholder.GetComponent<TextMeshProUGUI>().text = "생년월일을 입력해 주세요";
        }
    }
}
