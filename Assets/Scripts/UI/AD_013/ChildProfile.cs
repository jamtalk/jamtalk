using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildProfile : MonoBehaviour
{
    public Text textName;
    public Text textBirth;
    public Toggle toggle;
    public Image checkImage;
    public bool isCheck => toggle.isOn;

    private void Awake()
    {
        toggle.onValueChanged.AddListener((value) =>
        {
            checkImage.gameObject.SetActive(value);
        });
    }
}
