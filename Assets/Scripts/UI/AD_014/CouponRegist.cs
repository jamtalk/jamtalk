using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CouponRegist : MonoBehaviour
{
    public TMP_InputField couponInput;
    public Button registButton;
    public Button exitButton;
    public Action<string> registAction;

    private void OnEnable()
    {
        couponInput.text = string.Empty;
    }

    private void Awake()
    {
        registButton.onClick.AddListener(() => RegistAction());
        exitButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void RegistAction()
    {
        if (couponInput.text == string.Empty)
            registAction?.Invoke(couponInput.text);
    }
}
