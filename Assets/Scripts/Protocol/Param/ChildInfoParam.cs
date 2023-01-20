using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInfoParam : UserParam
{
    protected override eAPIAct act => eAPIAct.child_info;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_name", "");

        return form;
    }
}
