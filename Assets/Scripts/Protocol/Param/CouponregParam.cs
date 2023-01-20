using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class CouponregParam : UserParam
{
    protected override eAPIAct act => eAPIAct.couponreg;

    public string cp_subject;
    public string cp_start;
    public string cp_end;
    public string cp_price;
    public string cp_mem;

    public CouponregParam(string cp_subject, string cp_start,string cp_end, string cp_price, string cp_mem)
    {
        this.cp_subject = cp_subject;
        this.cp_start = cp_start;
        this.cp_end = cp_end;
        this.cp_price = cp_price;
        this.cp_mem = cp_mem;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("cp_subject", cp_subject);
        form.AddField("cp_start", cp_start);
        form.AddField("cp_end", cp_end);
        form.AddField("cp_price", cp_price);
        form.AddField("cp_mem", cp_mem);

        return form;
    }
}
