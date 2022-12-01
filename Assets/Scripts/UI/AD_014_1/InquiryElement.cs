using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InquiryElement : MonoBehaviour
{
    public AnswerStatus answerStatus;
    public Button button;
    public Text titleText;
    public Action clickAction;

    private void Awake()
    {
        button.onClick.AddListener(() => clickAction?.Invoke());
    }

    public void Init(bool isDone)
    {
        answerStatus.ChangeStatus(isDone);
        titleText.text = isDone ? "답변완료" : "답변대기";
    }
}
