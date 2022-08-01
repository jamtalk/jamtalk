using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MoleElement305 : MonoBehaviour
{
    public Image image;
    public Text text;
    
    public void Init(string value)
    {
        text.text = value.ToString().ToLower();
    }
}