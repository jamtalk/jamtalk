using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleText505 : MonoBehaviour
{
    public Toggle toggle;
    public Text text;

    public void Init(string value, bool isOn)
    {
        text.text = value;
        toggle.isOn = isOn;
    }
}
