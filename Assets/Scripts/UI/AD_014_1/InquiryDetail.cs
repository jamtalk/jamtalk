using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InquiryDetail : MonoBehaviour
{
    public Text textQuestionTitle;
    public Text textQuestionDate;
    public Text textAnswerTitle;
    public Text textAnswerDate;
    public Text textAnswerComment;
    public AnswerStatus answerStatus;
    public Button confirmButton;

    private void Awake()
    {
        confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void Init(BoardData data)
    {
        textQuestionTitle.text = data.wr_subject;
        textQuestionDate.text = data.wr_datetime;
        textAnswerTitle.text = data.wr_coment_title;
        textAnswerDate.text = data.wr_coment_datetime;
        textAnswerComment.text = data.wr_coment_detail;
        answerStatus.Init(data);
        gameObject.SetActive(true);
    }
}
