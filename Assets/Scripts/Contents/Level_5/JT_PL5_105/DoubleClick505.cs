using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClick505 : DoubleClickButton
{
    public Text text;

    public void Init(string value)
    {
        text.text = value;
    }
}
