using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordElement405 : MonoBehaviour
{
    public Text text;
    public Text texts;
    public Button currentButton;
    public Button worngButton;

    public void Init(string value)
    {
        text.text = value;
    }
    public void Init(eDigraphs value)
    { 
        text.text = value.ToString().ToLower();
        texts.text = value.ToString();
    }
}
