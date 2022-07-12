using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClick306 : DoubleClickButton
{
    public Text textValue;

    public void Init(eDigraphs digraphs)
    {
        var value = digraphs.ToString().ToLower();
        textValue.text = value;
        name = value;
    }
}
