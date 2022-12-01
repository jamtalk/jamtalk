using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InquiryPage : MonoBehaviour
{
    public InquiryRegist inquiryRegist;
    public InquiryList inquiryList;
    public Toggle registToggle;
    public Toggle listToggle;

    private void Awake()
    {
        registToggle.onValueChanged.AddListener((value) => ToggleAction(value)) ;
    }

    private void ToggleAction(bool value)
    {
        inquiryRegist.gameObject.SetActive(value);
        inquiryList.gameObject.SetActive(!value);
        inquiryList.inquiryDetail.gameObject.SetActive(false);
    }
}
