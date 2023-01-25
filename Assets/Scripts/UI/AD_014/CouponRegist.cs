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
        if (string.IsNullOrEmpty(couponInput.text))
            return;
        else
        {
            // 쿠폰 등록 param 설정 *_*
        }
    }
}
