using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClick306 : DoubleClickButton
{
    public Text textValue;
    public DigraphsWordsData value;
    public eDigraphs digraphs => value.Digraphs;

    public void Init(DigraphsWordsData value)
    {
        this.value = value;
        name = value.key;
        textValue.text = value.IncludedDigraphs.ToLower();
    }
}
