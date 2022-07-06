using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleClick302 : DoubleClickButton
{
    public Text textPhanix;
    public Image images;
    public bool isCheck = false;
    public DigraphsSource data { get; private set; }

    public void Init(DigraphsSource data)
    {
        this.data = data;
        textPhanix.text = data.type.ToString().ToLower();
        images.sprite = data.sprite;

        

    }
}
