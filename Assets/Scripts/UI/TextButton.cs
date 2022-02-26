using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class TextButton : MonoBehaviour
{
    public Button button => GetComponent<Button>();
    public Text text;
    public string value { get; private set; }
    public event Action<string> onClick;
    private void Awake()
    {
        button.onClick.AddListener(() => onClick?.Invoke(value));
    }
    public void Init(string value)
    {
        this.value = value;
        text.text = value;
    }
}
