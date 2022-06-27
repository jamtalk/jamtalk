using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitElement_408 : MonoBehaviour
{
    public Text text;
    public Image image;

    public void Init(string value)
    {
        text.text = value;
    }
    public void Init(string value , Sprite sprite)
    {
        text.gameObject.SetActive(false);
        text.text = value;
        image.sprite = sprite;
    }
}
