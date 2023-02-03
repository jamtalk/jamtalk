using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindAccount : MonoBehaviour
{
    public enum eTarget
    {
        FindID,
        FindPW,
        ChangePW,
    }

    public eTarget target = eTarget.FindID;
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

        SetReactionPopup(CheckVolid());
    }


    private void SetReactionPopup(bool isValid)
    {
        this.isValid = isValid;
        var value = string.Empty;

        if(target == eTarget.FindID)
        {
            if (isValid)
            {
                value = user.user_id.Replace("email:", string.Empty);
                value = string.Format("회원님의 아이디는 \"{0}\" 입니다.", value);
            }
            else
            {
                value = "입력한 정보와 일치하는 아이디가 없습니다.";
            }
        }
        else if (target == eTarget.FindPW)
        {
            if(isValid)
            {
                //value = user.pw;
                value = string.Format("회원님의 비밀번호는 \"{0}\" 입니다.", value);
            }
            else
            {
                value = "입력한 정보가 일치하지 않습니다.";
            }
        }
        else if (target == eTarget.ChangePW)
        {
            if (isValid)
            {
                var param = new MemberInfoParam(user, MemberInfoParam.eMemberInfo.user_pw, firstInput.text);

                RequestManager.Instance.RequestAct(param, (res) =>
                {
                    var result = res.GetResult<ActRequestResult>();

                    if (result.code != eErrorCode.Success)
                    {
                        Debug.Log(result.msg);
                    }
                    else
                    {
                        value = "입력하신 비밀번호로 변경되었습니다.";
                    }
                });
            }
            else
            {
                value = "입력하신 비밀번호가 일치하지 않습니다.";
            }
        }


        reactionPopup.gameObject.SetActive(true);
        exitPopupText.text = value;
    }

    private bool CheckVolid()
    {
        var isValid = false;

        if (target == eTarget.FindID)
            isValid = firstInput.text == user.name && secondInput.text == user.birth;

        else if (target == eTarget.FindPW)
            isValid = firstInput.text == user.user_id && secondInput.text == user.name;

        else if (target == eTarget.ChangePW)
            isValid = firstInput.text == secondInput.text;

        return isValid;
    }


    public void Init(eTarget target)
    {
        this.target = target;
        gameObject.SetActive(true);

        firstInput.text = string.Empty;
        secondInput.text = string.Empty;

        if(target == eTarget.FindPW)
        {
            title.text = "비밀번호 찾기";
            detail.text = "비밀번호를 찾을 아이디의 정보를 입력해주세요.";
            firstTitle.text = "아이디";
            firstInput.placeholder.GetComponent<TextMeshProUGUI>().text = "아이디를 입력해 주세요";
            secondTitle.text = "실명";
            secondInput.placeholder.GetComponent<TextMeshProUGUI>().text = "실명을 입력해 주세요";
        }
        else if( target == eTarget.ChangePW)
        {
            title.text = "비밀번호 변경";
            var nick = UserDataManager.Instance.CurrentUser.nick;
            var detailString = string.Format("{0}님\n비밀번호를 변경합니다.", nick);
            detail.text = detailString;
            firstTitle.text = "변경할 비밀번호";
            firstInput.placeholder.GetComponent<TextMeshProUGUI>().text = "비밀번호를 입력해 주세요";
            secondTitle.text = "변경할 비밀번호 확인";
            secondInput.placeholder.GetComponent<TextMeshProUGUI>().text = "비밀번호를 한번 더 입력해 주세요";
        }
        else if (target == eTarget.FindID)
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
