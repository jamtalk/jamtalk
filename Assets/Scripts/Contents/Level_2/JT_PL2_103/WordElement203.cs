using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordElement203 : MonoBehaviour
{
    public Text textValue;
    private bool _visible;
    public bool visible
    {
        get => _visible;
        set
        {
            _visible = value;
            var color = Color.white;
            color.a = value ? 1f : .3f;
            GetComponent<Image>().color = color;
            textValue.color = value ? Color.black : color;
        }
    }
    public RectTransform textRT;
    public Image image;
    public string value { get; private set; }
    public VowelWordsData data { get; private set; }
    public void Init(VowelWordsData data)
    {
        this.data = data;
        value = data.key;
        value = value;
        textValue.text = value;
        name = value;
    }
    public void Init(string data)
    {
        value = data;
        value = value;
        textValue.text = value;
        name = value;
    }
    public void SetSize()
    {
        var width = textRT.rect.width;
        var height = textRT.rect.height;
        var size = GetComponent<RectTransform>().sizeDelta;
        size.x = width + 20f;
        if(size.y < 0)
            size.y = height * -1;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}