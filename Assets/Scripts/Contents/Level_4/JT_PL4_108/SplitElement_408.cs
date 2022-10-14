using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitElement_408 : MonoBehaviour
{
    public Text text;
    public Image image;
    [SerializeField]
    public string value;

    public void Init(string value)
    {
        this.value = value;
        text.text = value;
    }
    public void Init(string value, Sprite sprite, bool isDigraphs = false)
    {
        this.value = value;

        text.gameObject.SetActive(false);
        text.text = value;
        if(isDigraphs)
            text.color = Color.red;
        image.sprite = sprite;
    }
}
