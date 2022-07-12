using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordElement121 : MonoBehaviour
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
            color.a = value ? 1 : .3f;
            GetComponent<Image>().color = color;
            textValue.color = color;
        }
    }
    public RectTransform textRT => textValue.GetComponent<RectTransform>();
    public string value { get; private set; }
    public void Init(string value)
    {
        this.value = value;
        textValue.text = value;
        name = value;
    }
    public void SetSize()
    {
        var width = textRT.rect.width;
        var size = GetComponent<RectTransform>().sizeDelta;
        size.x = width+20f;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}
