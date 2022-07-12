using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButton510 : TextButton
{
    public Image image;
    public event System.Action<DigraphsWordsData> onClickData;
    public DigraphsWordsData data { get; private set; }
    private void Awake()
    {
        button.onClick.AddListener(() => onClickData?.Invoke(data));
    }
    public void Init(DigraphsWordsData data)
    {
        base.Init(data.key);
        this.data = data;
        image.sprite = data.sprite;
        image.preserveAspect = true;
    }
}