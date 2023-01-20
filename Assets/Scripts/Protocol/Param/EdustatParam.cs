using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdustatParam : UserParam
{
    protected override eAPIAct act => eAPIAct.edustat;

    public string type;
    public string month;

    public EdustatParam(string type, string month)
    {
        this.type = type;
        this.month = month;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("type", type);
        form.AddField("month", month);

        return form;
    }
}
