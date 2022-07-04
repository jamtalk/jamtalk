using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButton510 : TextButton
{
    public Image image;
    public event System.Action<DigraphsSource> onClickData;
    public DigraphsSource data { get; private set; }
    private void Awake()
    {
        button.onClick.AddListener(() => onClickData?.Invoke(data));
    }
    public void Init(DigraphsSource data)
    {
        base.Init(data.value);
        this.data = data;
        image.sprite = data.sprite;
        image.preserveAspect = true;
    }
}