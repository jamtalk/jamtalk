using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClick505 : DoubleClickButton
{
    public Text text;
    public int number;
    public bool isCheck { get; private set; }

    public void Init(string value, int number, bool isCheck)
    {
        text.text = value;
        this.number = number;
        this.isCheck = isCheck;
    }
}
