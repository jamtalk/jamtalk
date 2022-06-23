using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleElement305 : MonoBehaviour
{
    public Image image;
    public Text text;

    public void Init(Mole data)
    {
        image.sprite = data.color;
        text.text = data.digraphs.ToString();
    }
}
