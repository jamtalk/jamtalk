using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class CouponregParam : UserParam
{
    protected override eAPIAct act => eAPIAct.couponreg;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("cp_subject", "");
        form.AddField("cp_start", "");
        form.AddField("cp_end", "");
        form.AddField("cp_price", "");
        form.AddField("cp_mem", "");

        return form;
    }
}
