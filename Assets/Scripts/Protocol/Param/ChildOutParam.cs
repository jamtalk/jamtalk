using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOutParam : UserParam
{
    public override eAPIAct act => eAPIAct.childout;

    public string name;
    public bool display;
    public string level;

    public ChildOutParam(string name, bool display, string level)
    {
        this.name = name;
        this.display = display;
        this.level = level;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("name", name);
        form.AddField("display", Convert.ToInt16(display));
        form.AddField("level", level);

        return form;
    }
}
