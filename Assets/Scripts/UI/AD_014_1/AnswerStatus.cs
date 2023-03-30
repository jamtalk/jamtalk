using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerStatus : MonoBehaviour
{
    public Color colorAnswered;
    public Color colorWait;
    public Image image;
    public Text text;

    public void Init(BoardData data)
    {
        image.color = data.isAnswered ? colorAnswered : colorWait;
        text.text = data.isAnswered ? "답변완료" : "답변대기";
    }
}
