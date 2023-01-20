using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOutParam : UserParam
{
    protected override eAPIAct act => eAPIAct.childout;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("name", "");
        form.AddField("display", "");
        form.AddField("level", "");

        return form;
    }
}
