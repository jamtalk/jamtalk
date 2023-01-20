using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOutParam : UserParam
{
    protected override eAPIAct act => eAPIAct.childout;

    public string name;
    public string display;
    public string level;

    public ChildOutParam(string name, string display, string level)
    {
        this.name = name;
        this.display = display;
        this.level = level;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("name", name);
        form.AddField("display", display);
        form.AddField("level", level);

        return form;
    }
}
