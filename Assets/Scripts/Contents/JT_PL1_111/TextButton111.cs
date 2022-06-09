using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButton111 : TextButton
{
    public Image image;
    public event System.Action<WordsData.WordSources> onClickData;
    public WordsData.WordSources data { get; private set; }
    private void Awake()
    {
        button.onClick.AddListener(() => onClickData?.Invoke(data));
    }
    public void Init(WordsData.WordSources data)
    {
        base.Init(data.value);
        this.data = data;
        image.sprite = data.sprite;
        image.preserveAspect = true;
    }
}
