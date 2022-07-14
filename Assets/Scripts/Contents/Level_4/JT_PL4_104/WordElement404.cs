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

    public Question4_104 data;
    public bool isOpen { get; private set; }

    public void Init(Question4_104 data)
    {
        isOpen = false;
        this.data = data;
        text.text = data.text;
    }

    public void Open()
    {
        isOpen = true;
        closeImage.gameObject.SetActive(false);
        openImage.gameObject.SetActive(true);
    }
    public void Close()
    {
        isOpen = false;
        closeImage.gameObject.SetActive(true);
        openImage.gameObject.SetActive(false);
    }
    public void Correct()
    {
        charactor.gameObject.SetActive(false);
    }
}
