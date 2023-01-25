using System;
using UnityEngine;

public class CouponregParam : UserParam
{
    protected override eAPIAct act => eAPIAct.couponreg;

    public string cp_subject;
    public DateTime cp_start;
    public DateTime cp_end;
    public int cp_price;
    public string cp_mem;

    public CouponregParam(string cp_subject, DateTime cp_start, DateTime cp_end, int cp_price, string cp_mem)
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
        form.AddField("cp_start", cp_start.ToString("yyyy-mm-dd"));
        form.AddField("cp_end", cp_end.ToString("yyyy-mm-dd"));
        form.AddField("cp_price", cp_price);
        form.AddField("cp_mem", cp_mem);

        return form;
    }
}
