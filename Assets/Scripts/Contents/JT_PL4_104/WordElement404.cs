using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordElement404 : MonoBehaviour
{
    public Text text;
    public Image closeImage;
    public Image openImage;
    public GameObject charactor;
    public Button button;

    public void Init(eDigraphs value)
    {
        text.text = value.ToString().ToLower();
    }
    public void Open()
    {
        closeImage.gameObject.SetActive(false);
        openImage.gameObject.SetActive(true);
    }
    public void Correct()
    {
        charactor.gameObject.SetActive(false);
    }
}
