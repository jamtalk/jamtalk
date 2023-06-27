using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WordElement404 : MonoBehaviour
{
    public Text text;
    public Image textImage;
    public Image closeImage;
    public Image openImage;
    public GameObject[] charactors;
    public Button button;

    public Question4_104 data;
    public bool isOpen { get; private set; }
    private GameObject charactor;

    public void Init(Question4_104 data)
    {
        charactor = charactors.OrderBy(x => Random.Range(0, 100)).First();
        charactor.gameObject.SetActive(true);
        isOpen = false;
        this.data = data;
        text.text = data.text;
    }

    public void Open()
    {
        isOpen = true;
        closeImage.gameObject.SetActive(false);
        openImage.gameObject.SetActive(true);
        textImage.gameObject.SetActive(true);
    }
    public void Close()
    {
        isOpen = false;
        closeImage.gameObject.SetActive(true);
        openImage.gameObject.SetActive(false);
    }
    public void Correct()
    {
        textImage.gameObject.SetActive(false);
        charactor.gameObject.SetActive(false);
    }
}
