using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InquiryDetail : MonoBehaviour
{
    public TMP_InputField titleInput;
    public TMP_InputField detailInput;
    public TMP_InputField answerInput;
    public AnswerStatus answerStatus;
    public Button confirmButton;

    private void Awake()
    {
        confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
    }


    /// <summary>
    /// 데이터 받아서 상태 변경 / 답변, 제목, 질문 설정
    /// </summary>
    public void Init()
    {
        Debug.Log("init");
        //titleInput.text = 
        //detailInput.text =
        //answerInput.text =
        //answerStatus.ChangeStatus();
        gameObject.SetActive(true);
    }
}
