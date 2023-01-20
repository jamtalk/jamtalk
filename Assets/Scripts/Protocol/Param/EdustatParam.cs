using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdustatParam : UserParam
{
    protected override eAPIAct act => eAPIAct.edustat;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("type", "");
        form.AddField("month", "");

        return form;
    }
}
