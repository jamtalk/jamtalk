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
        InitData(UserDataManager.Instance.CurrentChild);
    }

    public void SetEdit(bool value)
    {
        InitData(UserDataManager.Instance.CurrentChild);
        toggle.gameObject.SetActive(value);
        foreach (var item in inputFields)
            item.gameObject.SetActive(value);

        inputName.gameObject.SetActive(value);
        inputBirth.gameObject.SetActive(value);
        textName.gameObject.SetActive(!value);
        textBirth.gameObject.SetActive(!value);
    }
    public void InitData(ChildInfoData child)
    {
        inputName.text = child.name;
        inputBirth.text = child.jumin;
        Debug.LogFormat("{0} : {1} 편집 설정", child.name, child.jumin);

        textName.text = child.name;
        textBirth.text = child.jumin;
    }

    
}
