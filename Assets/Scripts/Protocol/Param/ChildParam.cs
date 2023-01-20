using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildParam : UserParam
{
    protected override eAPIAct act => eAPIAct.child;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("name", "");
        form.AddField("jumin", "");
        form.AddField("display", "");
        form.AddField("level", "");

        return form;
    }
}
