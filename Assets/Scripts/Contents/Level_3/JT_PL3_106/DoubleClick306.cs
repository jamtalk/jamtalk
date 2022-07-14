using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClick306 : DoubleClickButton
{
    public Text textValue;
    public eDigraphs eDigraphs { get; private set; }

    public void Init(eDigraphs digraphs)
    {
        eDigraphs = digraphs;
        var value = digraphs.ToString().ToLower();
        textValue.text = value;
        name = value;
    }
}
