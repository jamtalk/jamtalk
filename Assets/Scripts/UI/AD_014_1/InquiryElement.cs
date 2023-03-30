using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class InquiryElement : MonoBehaviour
{
    public AnswerStatus answerStatus;
    public Button button;
    public Text textTitle;
    public Text textDate;
    public UnityEvent<BoardData> onClick;
    private BoardData data;

    private void Awake()
    {
        button.onClick.AddListener(() => onClick?.Invoke(data));
    }

    public void Init(BoardData data)
    {
        this.data = data;
        answerStatus.Init(data);
        textTitle.text = data.wr_subject;
        textDate.text = data.wr_datetime;
        button.interactable = data.isAnswered;
    }
}
