using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButton111 : TextButton
{
    public Image image;
    public event System.Action<AlphabetWordsData> onClickData;
    public AlphabetWordsData data { get; private set; }
    private void Awake()
    {
        button.onClick.AddListener(() => onClickData?.Invoke(data));
    }
    public void Init(AlphabetWordsData data)
    {
        base.Init(data.key);
        this.data = data;
        image.sprite = data.sprite;
        image.preserveAspect = true;
    }
}
