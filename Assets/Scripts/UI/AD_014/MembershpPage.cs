using System;
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

    private int monthPrice => 29000;
    private int yearthPrice => 290000;

    private void Awake()
    {
        couponButton.onClick.AddListener(() => couponRegist.gameObject.SetActive(true));

        //monthButton.onClick.AddListener(() => Couponreg());
        //yearthButton.onClick.AddListener(() => Couponreg(false));
    }

    private void Couponreg(bool isMonth = true)
    {
        var price = 0;
        var endDate = DateTime.Now;

        if(isMonth)
        {
            price = monthPrice;
            endDate.AddDays(30);
        }
        else
        {
            price = yearthPrice;
            endDate.AddDays(365);
        }

        // 구독관련 Param 추가 시 변경 *_*
        var param = new CouponregParam(string.Empty, DateTime.Now, endDate, price, string.Empty);

        RequestManager.Instance.Request(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            if(result.code != eErrorCode.Success)
            {
                Debug.Log(result.code);
                AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
            }
            else
            {

            }
        });
    }
}
