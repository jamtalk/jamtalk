using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardParam : UserParam
{
    public override eAPIAct act => eAPIAct.award;

    public string app_token;
    public string type;
    public DateTime regdate;
    public string award;
    public string level;
    public DateTime end_time;
    public string total_score;

    public AwardParam(string app_token, string type, DateTime regdate, string award, string level, DateTime end_time, string total_score) : base()
    {
        this.app_token = app_token;
        this.type = type;
        this.regdate = regdate;
        this.award = award;
        this.level = level;
        this.end_time = end_time;
        this.total_score =total_score;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("app_token", app_token);
        form.AddField("type", type);
        form.AddField("regdate", regdate.ToString("yyyy-MM-dd H:mm:ss"));
        form.AddField("award", award);
        form.AddField("level", level);
        form.AddField("end_time", end_time.ToString("yyyy-MM-dd H:mm:ss"));
        form.AddField("total_score", total_score);

        return form;
    }
}