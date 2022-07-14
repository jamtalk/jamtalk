using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextElement206 : Text
{
    public VowelWordsData data { get; private set; }

    public void Init(VowelWordsData data)
    {
        this.data = data;
        text = data.key;
        name = data.key;
    }
}
