using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardParam : UserParam
{
    protected override eAPIAct act => eAPIAct.award;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("app_token", "");
        form.AddField("type", "0");
        form.AddField("regdate", "2021-06-29");
        form.AddField("award", "");
        form.AddField("level", "");
        form.AddField("end_time", "");
        form.AddField("total_score", "");

        return form;
    }
}