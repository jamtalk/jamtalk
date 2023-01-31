using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System;

public class Level_logParam : UserParam
{
    protected override eAPIAct act => eAPIAct.level_log;

    public DateTime regdate;
    public int level;
    public int before;

    public Level_logParam(DateTime regdate, int level, int before)
    {
        this.regdate = regdate;
        this.level = level;
        this.before = before;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();

        form.AddField("regdate", regdate.ToString("yyyy-MM-dd H:mm:ss"));
        form.AddField("level", level);
        form.AddField("before", before);

        return form;
    }
}
