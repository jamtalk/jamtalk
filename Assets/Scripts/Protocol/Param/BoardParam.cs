using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class BoardParam : UserParam
{
    protected override eAPIAct act => eAPIAct.board;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("bbs", "");
        form.AddField("uid", "");

        return form;
    }
}
