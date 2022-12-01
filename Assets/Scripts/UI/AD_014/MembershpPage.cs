using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MembershpPage : MonoBehaviour
{
    public Button couponButton;
    public Button monthButton;
    public Button yearthButton;
    public CouponRegist couponRegist;

    private void Awake()
    {
        couponButton.onClick.AddListener(() => couponRegist.gameObject.SetActive(true));
    }
}
