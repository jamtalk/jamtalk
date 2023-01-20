using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class MemberOutParam : UserParam
{
    protected override eAPIAct act => eAPIAct.memberout;

    public string user_name;
    public string user_hp;

    public MemberOutParam(string user_name, string user_hp)
    {
        this.user_name = user_name;
        this.user_hp = user_hp;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_name", user_name);
        form.AddField("user_hp", user_hp);

        return form;
    }
}
