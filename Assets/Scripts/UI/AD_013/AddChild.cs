using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddChild : MonoBehaviour
{
    public TMP_InputField inputName;
    public TMP_InputField inputBirth;
    public Toggle maleToggle;
    public Toggle femaleToggle;
    public Toggle termsToggle;
    public Button termsMoreButton;
    public Button confirmButton;
    public GameObject terms;

    private void Awake()
    {
        //termsMoreButton.onClick.AddListener(() => terms.SetActive(true));
        termsMoreButton.onClick.AddListener(() => Application.OpenURL("https://jamtalk.live/privacy"));
        confirmButton.onClick.AddListener(() => ConfirmAction());
    }

    private void ConfirmAction()
    {
    }

    public void ExitAction()
    {
        inputName.text = string.Empty;
        inputBirth.text = string.Empty;
        termsToggle.isOn = false;
        maleToggle.isOn = false;
        femaleToggle.isOn = false;
        gameObject.SetActive(false);
    }

}
