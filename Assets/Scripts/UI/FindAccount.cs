using System.Collections;
using System.Collections.Generic;
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

    

    private void Awake()
    {
        exitButton.onClick.AddListener(() => gameObject.SetActive(false));
        confirmButton.onClick.AddListener(() => ConfirmAction());
    }

    private void ConfirmAction()
    {
        if (firstInput.text == string.Empty) return;
        if (secondInput.text == string.Empty) return;

        SendReqeust(target);
    }

    private void SendReqeust(eTarget target)
    {
        if (target == eTarget.FindID)
        {
            // 아이디 찾기 param
        }
        else if (target == eTarget.FindPW)
        {
            // 비밃번호 찾기 Param
        }
        else if (target == eTarget.ChangePW)
        {
            // 비밀번호 변경 param
        }
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
            firstTitle.text = "이메일";
            firstInput.placeholder.GetComponent<TextMeshProUGUI>().text = "이메일을 입력해 주세요";
            secondTitle.text = "이름";
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
