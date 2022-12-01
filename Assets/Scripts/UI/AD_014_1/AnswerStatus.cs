using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerStatus : MonoBehaviour
{
    public Image image;
    public Text text;

    public void ChangeStatus(bool value)
    {
        image.color = value ? Color.blue : Color.gray;
    }
}
