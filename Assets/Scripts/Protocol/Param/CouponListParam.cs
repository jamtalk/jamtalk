using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouponListParam : UserParam
{
    protected override eAPIAct act => eAPIAct.coupon_list;

    public CouponListParam() { }
}
