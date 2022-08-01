using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClick306 : DoubleClickButton
{
    public Text textValue;
    public string value;
    public eDigraphs digraphs { get; private set; }

    public void Init(string value)
    {
        this.value = value;
        textValue.text = value.ToLower();
        name = value;
        digraphs = (eDigraphs)Enum.Parse(typeof(eDigraphs), value.ToUpper());
    }
}
