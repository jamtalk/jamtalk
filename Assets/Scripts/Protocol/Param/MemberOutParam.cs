using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class MemberOutParam : UserParam
{
    protected override eAPIAct act => eAPIAct.memberout;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_name", "");
        form.AddField("user_hp", "");

        return form;
    }
}
