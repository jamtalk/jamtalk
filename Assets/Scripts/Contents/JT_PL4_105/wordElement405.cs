using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordElement405 : MonoBehaviour
{
    public Text text;
    public Text pairText;
    public Button digraphsButton;
    public Button pairButton;

    public void Init(string value)
    {
        text.text = value;
    }
    public void Init(string digraphs, string pair)
    { 
        text.text = digraphs;
        pairText.text = pair;
    }
}
