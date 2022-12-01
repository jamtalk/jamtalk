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

    public void Init(bool isAdd = false)
    {
        this.isAdd = isAdd;
    }

    private void ClickAction()
    {
        if(isAdd)
        {
            addAction?.Invoke();
        }
    }

}
