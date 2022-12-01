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
        confirmButton.onClick.AddListener(() => ConfirmInqury());
    }

    /// <summary>
    /// 질문사항 등록
    /// </summary>
    private void ConfirmInqury()
    {
        if (titleInput.text == string.Empty)
        {
            return;
            //PopupManager.Instance.ShowGuidance("제목을 입력하세요.");
        }
        if (detailInput.text == string.Empty)
        {
            return;
            //PopupManager.Instance.ShowGuidance("내용을 입력하세요.");
        }


        /// 질문사항 등록
        ///

        registPopup.gameObject.SetActive(true);
        var exitButton = registPopup.GetComponentInChildren<Button>();
        exitButton.onClick.AddListener(() =>
        {
            registPopup.gameObject.SetActive(false);
            titleInput.text = string.Empty;
            detailInput.text = string.Empty;
        });
    }
}
