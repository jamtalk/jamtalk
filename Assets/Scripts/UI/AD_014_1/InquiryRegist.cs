using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InquiryRegist : MonoBehaviour
{
    public TMP_InputField titleInput;
    public TMP_InputField detailInput;
    public Button confirmButton;
    public GameObject registPopup;

    private void Awake()
    {
        confirmButton.onClick.AddListener(ConfirmInqury);
    }

    /// <summary>
    /// 질문사항 등록
    /// </summary>
    private void ConfirmInqury()
    {
        if (titleInput.text == string.Empty)
        {
            AndroidPluginManager.Instance.Toast("제목을 입력하세요.");
            return;
        }
        if (detailInput.text == string.Empty)
        {
            AndroidPluginManager.Instance.Toast("내용을 입력하세요.");
            return;
        }

        var param = new BoardWriteParam(eBoardType.qa, titleInput.text, detailInput.text);

        RequestManager.Instance.Request(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            if(result.code != eErrorCode.Success)
            {
                Debug.Log(result.code);
                AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
            }
            else
            {
                registPopup.gameObject.SetActive(true);
                var exitButton = registPopup.GetComponentInChildren<Button>();
                exitButton.onClick.AddListener(() =>
                {
                    registPopup.gameObject.SetActive(false);
                    titleInput.text = string.Empty;
                    detailInput.text = string.Empty;
                });
            }
        });
    }
}
