using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildIconElement : MonoBehaviour
{
    public Image image;
    public Button button;
    public Text text;
    public Action addAction; 
    private bool isAdd = false;

    private void Awake()
    {
        button.onClick.AddListener(() => ClickAction());
    }

    public void Init(ChildInfoData data)
    {
        if (data == null)
        {
            isAdd = true;
            return;
        }

        text.text = data.name;
    }

    private void ClickAction()
    {
        if(isAdd)
        {
            addAction?.Invoke();
        }
    }

}
