using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exists_emailParam : ActParam
{
    protected override eAPIAct act => eAPIAct.exists_email;

    public string user_email;
    public Exists_emailParam(string user_email)
    {
        this.user_email = user_email;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_email", user_email);

        return form;
    }
}
