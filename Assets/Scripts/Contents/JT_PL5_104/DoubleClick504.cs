using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClick504 : DoubleClickButton
{
    public event System.Action<DigraphsWordsData> onClickData;
    public DigraphsWordsData data { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(() => onClickData?.Invoke(data));
    }
    public void Init(DigraphsWordsData data)
    {
        this.data = data;
    }
}
