using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildParam : UserParam
{
    public override eAPIAct act => eAPIAct.child;

    public string name;
    public string jumin;
    public bool display;
    public int level;

    public ChildParam(string name, string jumin, bool display = false, int level = 1)
    {
        user_id = UserDataManager.Instance.CurrentUser.user_id;
        this.name = name;
        this.jumin = jumin;
        this.display = display;
        this.level = level;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("name", name);
        form.AddField("jumin", jumin);
        int display = Convert.ToInt16(this.display);
        form.AddField("display", Convert.ToInt16(display));
        form.AddField("level", level);

        return form;
    }
}
