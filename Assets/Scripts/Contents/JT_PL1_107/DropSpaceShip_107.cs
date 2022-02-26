using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropSpaceShip_107 : MonoBehaviour
{
    public Text text;
    public RectTransform point;
    public bool isConnected = false;
    public Button button;
    public Action<string> onClick;
    public string currentValue { get; private set; }
    public void Init(string value)
    {
        currentValue = value;
        isConnected = false;
        text.text = value;
        button.onClick.AddListener(() => onClick?.Invoke(text.text));
    }
}
