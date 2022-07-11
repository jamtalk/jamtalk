using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClick504 : DoubleClickButton
{
    public event System.Action<DigraphsSource> onClickData;
    public DigraphsSource data { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => onClickData?.Invoke(data));
    }
    public void Init(DigraphsSource data)
    {
        this.data = data;
    }
}
