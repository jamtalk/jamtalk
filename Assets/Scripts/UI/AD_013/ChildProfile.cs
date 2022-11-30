using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildProfile : MonoBehaviour
{
    public Text textName;
    public Text textBirth;
    public TMP_InputField inputName;
    public TMP_InputField inputBirth;
    public Toggle toggle;
    public Image checkImage;
    public GameObject[] inputFields;
    public bool isCheck => toggle.isOn;

    private void Awake()
    {
        toggle.onValueChanged.AddListener((value) =>
        {
            checkImage.gameObject.SetActive(value);
        });
    }

    public void SetEdit(bool value)
    {
        toggle.gameObject.SetActive(value);
        //foreach (var item in inputFields)
        //    item.gameObject.SetActive(value);

        //textName.gameObject.SetActive(!value);
        //textBirth.gameObject.SetActive(!value);
    }

    
}
