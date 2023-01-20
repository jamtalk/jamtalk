using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildParam : UserParam
{
    protected override eAPIAct act => eAPIAct.child;

    public string name;
    public string jumin;
    public string display;
    public string level;

    public ChildParam(string name, string jumin, string display, string level)
    {
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
        form.AddField("display", display);
        form.AddField("level", level);

        return form;
    }
}
