using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberInfoParam : UserParam
{
    protected override eAPIAct act => eAPIAct.memberinfo;

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_pw", "");
        form.AddField("user_name", "");
        form.AddField("user_hp", "");
        form.AddField("user_pic", "");
        form.AddField("on_push", "");
        form.AddField("on_event", "");
        form.AddField("device", "");
        form.AddField("device_ver", "");
        form.AddField("device_token", "");

        return form;
    }
}
