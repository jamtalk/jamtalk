using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClick504 : DoubleClickButton
{
    public DigraphsSource data { get; private set; }
    public void Init(DigraphsSource data)
    {
        this.data = data;
    }
}
